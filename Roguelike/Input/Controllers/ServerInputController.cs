using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Roguelike.Input.Processors;
using Roguelike.Model;
using Roguelike.Model.Mobs;
using Roguelike.Network;

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
            if (!clientStreams.ContainsKey(request.Login))
            {
                level.AddPlayer(request.Login);
                var levelSnapshot = level.Save().ToString();
                var initResponse = new ServerResponse {Type = ResponseType.Init, Level = levelSnapshot};
                await responseStream.WriteAsync(initResponse);
                clientStreams.Add(request.Login, responseStream);
            }
            else
            {
                var rejectResponse = new ServerResponse {Type = ResponseType.LoginExists};
                await responseStream.WriteAsync(rejectResponse);
            }

            while (true)
            {
            }
        }

        public override async Task<Empty> Move(InputRequest request, ServerCallContext context)
        {
            var key = KeyParser.ToConsoleKey(request.KeyInput);
            var login = request.Login;
            var character = level.GetCharacter(login);

            Console.WriteLine($"Receive {key.Key} {login}");
            
            foreach (var subscriber in subscribers)
            {
                subscriber.ProcessInput(key, character);
            }

            foreach (var targetLogin in clientStreams.Keys)
            {
                var response = new ServerResponse
                {
                    Type =  ResponseType.Move, 
                    Login = request.Login,
                    KeyInput = request.KeyInput
                };
                Console.WriteLine($"Sending {response.Type.ToString()} {response.KeyInput} to {targetLogin}");
                await clientStreams[targetLogin].WriteAsync(response);
            }

            return new Empty();
        }

        public async void Move(Mob mob, Position intentPosition)
        {
            foreach (var clientStream in clientStreams.Values)
            {
                var response = new ServerResponse
                {
                    Type =  ResponseType.MobMove,
                    Login = mob.Id.ToString(),
                    Pair = new Pair {X = intentPosition.X, Y = intentPosition.Y}
                };
                await clientStream.WriteAsync(response);
            }
        }
    }
}