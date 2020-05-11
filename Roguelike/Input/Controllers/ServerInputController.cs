using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Roguelike.Input.Processors;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.Mobs;
using Roguelike.Model.PlayerModel;
using Roguelike.Network;

namespace Roguelike.Input.Controllers
{
    public class ServerInputController : ServerInputControllerService.ServerInputControllerServiceBase, 
        IMobMoveListener, IActionListener, IPlayerMoveListener
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
                var newPlayer = level.AddPlayerAtEmpty(request.Login);
                var levelSnapshot = level.Save().ToString();
                var initResponse = new ServerResponse {Type = ResponseType.Init, Level = levelSnapshot};
                await responseStream.WriteAsync(initResponse);
                
                foreach (var targetLogin in clientStreams.Keys)
                {
                    if (targetLogin == request.Login)
                    {
                        continue;
                    }
                    
                    var response = new ServerResponse
                    {
                        Type = ResponseType.PlayerJoin,
                        Login = request.Login,
                        Pair = new Pair {Y = newPlayer.Position.Y, X = newPlayer.Position.X}
                    };
                    Console.WriteLine($"Sending {response.Type.ToString()} {request.Login} to {targetLogin}");
                    await clientStreams[targetLogin].WriteAsync(response);
                }
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

        public override Task<Empty> Move(InputRequest request, ServerCallContext context)
        {
            var key = KeyParser.ToConsoleKey(request.KeyInput);
            var login = request.Login;
            var character = level.GetCharacter(login);

            Console.WriteLine($"Receive {key.Key} {login}");
            
            foreach (var subscriber in subscribers)
            {
                subscriber.ProcessInput(key, character);
            }

            return Task.FromResult(new Empty());
        }

        public async void MakeAction(AbstractPlayer player, ActionType actionType)
        {
            foreach (var targetLogin in clientStreams.Keys)
            {
                var response = new ServerResponse
                {
                    Type = ResponseType.Action,
                    Login = player.Login,
                    KeyInput = KeyParser.FromActionTypeToKeyInput(actionType)
                };
                Console.WriteLine($"Sending {response.Type.ToString()} {response.KeyInput} to {targetLogin}");
                await clientStreams[targetLogin].WriteAsync(response);
            }
        }

        public async void MovePlayer(AbstractPlayer player, Position intentPosition)
        {
            foreach (var targetLogin in clientStreams.Keys)
            {
                var response = new ServerResponse
                {
                    Type = ResponseType.Move,
                    Login = player.Login,
                    Pair = new Pair {X = intentPosition.X, Y = intentPosition.Y}
                };
                Console.WriteLine($"Sending {response.Type.ToString()} " +
                                  $"{intentPosition.Y} {intentPosition.X} to {targetLogin}");
                await clientStreams[targetLogin].WriteAsync(response);
            }
        }

        public async void Move(Mob mob, Position intentPosition)
        {
            foreach (var targetLogin in clientStreams.Keys)
            {
                var response = new ServerResponse
                {
                    Type =  ResponseType.MobMove,
                    Login = mob.Id.ToString(),
                    Pair = new Pair {X = intentPosition.X, Y = intentPosition.Y}
                };
                Console.WriteLine($"Sending {response.Type.ToString()} {response.Pair.Y} {response.Pair.X} to {targetLogin}");
                await clientStreams[targetLogin].WriteAsync(response);
            }
        }
    }

    public interface IPlayerMoveListener
    {
        void MovePlayer(AbstractPlayer player, Position intentPosition);
    }

    public interface IActionListener
    {
        void MakeAction(AbstractPlayer player, ActionType actionType);
    }
}