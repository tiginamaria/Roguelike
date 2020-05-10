using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Roguelike.Input.Processors;
using Roguelike.Model;
using Roguelike.Model.Mobs;

namespace Roguelike.Input.Controllers
{
    public class ServerInputController : ServerInputControllerService.ServerInputControllerServiceBase, IMobMoveListener
    {
        private readonly Level level;
        private readonly List<IInputProcessor> subscribers = new List<IInputProcessor>();
        private readonly Dictionary<string, IServerStreamWriter<ServerResponse>> clientStreams =
            new Dictionary<string, IServerStreamWriter<ServerResponse>>();

        public ServerInputController(Level level)
        {
            this.level = level;
        }

        public void AddInputProcessor(IInputProcessor inputProcessor)
        {
            subscribers.Add(inputProcessor);
        }
        
        public override async Task Login(LoginRequest request, IServerStreamWriter<ServerResponse> responseStream,
            ServerCallContext context)
        {
            if (clientStreams.ContainsKey(request.Login))
            {
                clientStreams.Add(request.Login, responseStream);
                var levelSnapshot = level.Save().ToString();
                var initResponse = new ServerResponse {Type = ResponseType.Init, Level = levelSnapshot};
                await responseStream.WriteAsync(initResponse);
            }
            else
            {
                var rejectResponse = new ServerResponse {Type = ResponseType.LoginExists};
                await responseStream.WriteAsync(rejectResponse);
            }
        }

        public override async Task<Empty> Move(InputRequest request, ServerCallContext context)
        {
            var key = GetKey(request.KeyInput);
            foreach (var subscriber in subscribers)
            {
                subscriber.ProcessInput(key);
            }

            foreach (var clientStream in clientStreams.Values)
            {
                var response = new ServerResponse
                {
                    Type =  ResponseType.Move, 
                    Login = request.Login,
                    KeyInput = request.KeyInput
                };
                await clientStream.WriteAsync(response);
            }

            return new Empty();
        }

        private ConsoleKeyInfo GetKey(KeyInput requestKeyInput)
        {
            return requestKeyInput switch
            {
                KeyInput.A => new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false),
                KeyInput.F =>  new ConsoleKeyInfo('f', ConsoleKey.F, false, false, false),
                KeyInput.H =>  new ConsoleKeyInfo('h', ConsoleKey.H, false, false, false),
                KeyInput.AltA =>  new ConsoleKeyInfo('a', ConsoleKey.A, false, true, false),
                KeyInput.AltF =>  new ConsoleKeyInfo('f', ConsoleKey.F, false, true, false),
                KeyInput.AltH =>  new ConsoleKeyInfo('h', ConsoleKey.H, false, true, false),
                KeyInput.Up => new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, false, false, false),
                KeyInput.Down => new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false),
                KeyInput.Left => new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false),
                KeyInput.Right => new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false),
                KeyInput.None => new ConsoleKeyInfo('\0', ConsoleKey.NoName, false, false, false),
                KeyInput.Exit => new ConsoleKeyInfo('\0', ConsoleKey.Escape, false, false, false),
                _ => new ConsoleKeyInfo('\0', ConsoleKey.NoName, false, false, false),
            };
        }

        public async void Move(Mob mob, Position intentPosition)
        {
            foreach (var clientStream in clientStreams.Values)
            {
                var response = new ServerResponse
                {
                    Type =  ResponseType.MobMove, 
                    Pair = new Pair {X = intentPosition.X, Y = intentPosition.Y}
                };
                await clientStream.WriteAsync(response);
            }
        }
    }
}