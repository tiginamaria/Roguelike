using System;
using System.Collections.Concurrent;
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
        IMobMoveListener, IActionListener, IPlayerMoveListener, IUpdatable
    {
        private readonly Level level;
        private readonly List<IInputProcessor> subscribers = new List<IInputProcessor>();

        private readonly ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>> responses = 
            new ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>>();
        private readonly ConcurrentQueue<InputRequest> requests = new ConcurrentQueue<InputRequest>();
        private readonly ConcurrentQueue<LoginRequest> loginRequests = new ConcurrentQueue<LoginRequest>();
        private readonly ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>> loginResponses = 
            new ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>>();
        
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
            try
            {
                loginRequests.Enqueue(request);

                while (true)
                {
                    if (!loginResponses.ContainsKey(request.Login))
                    {
                        continue;
                    }
                    
                    if (loginResponses[request.Login].TryDequeue(out var loginResponse))
                    {
                        await SendLoginResponses(request.Login, responseStream, loginResponse);
                    }

                    if (!responses.ContainsKey(request.Login))
                    {
                        continue;
                    }

                    if (responses[request.Login].TryDequeue(out var response))
                    {
                        if (response.KeyInput == KeyInput.Exit && response.Login == request.Login)
                        {
                            loginResponses.Remove(request.Login, out _);
                            responses.Remove(request.Login, out _);
                            break;
                        }
                        await responseStream.WriteAsync(response);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task SendLoginResponses(string targetLogin, 
            IServerStreamWriter<ServerResponse> responseStream,
            ServerResponse loginResponse)
        {
            if (loginResponse.Type == ResponseType.Init || loginResponse.Type == ResponseType.LoginExists)
            {
                Console.WriteLine($"Send login response {targetLogin} " +
                                  $"{loginResponse.Type.ToString()} to {targetLogin}");
                await responseStream.WriteAsync(loginResponse);
                if (loginResponse.Type == ResponseType.Init)
                {
                    responses.TryAdd(targetLogin, new ConcurrentQueue<ServerResponse>());
                }
            }
            else if (loginResponse.Type == ResponseType.PlayerJoin)
            {
                Console.WriteLine(
                    $"Send login response {loginResponse.Type.ToString()} " +
                    $"{loginResponse.Login} to {targetLogin}");
                await responseStream.WriteAsync(loginResponse);
            }
        }

        public override Task<Empty> Move(InputRequest request, ServerCallContext context)
        {
            requests.Enqueue(request);
            return Task.FromResult(new Empty());
        }

        public void Update()
        {
            if (loginRequests.TryDequeue(out var loginRequest))
            {
                ProcessLoginRequest(loginRequest);
            }
            if (requests.TryDequeue(out var request))
            {
                ProcessRequest(request);
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void ProcessLoginRequest(LoginRequest request)
        {
            if (!loginResponses.ContainsKey(request.Login))
            {
                var newPlayer = level.AddPlayerAtEmpty(request.Login);
                var levelSnapshot = level.Save().ToString();
                
                var initResponse = new ServerResponse {
                    Type = ResponseType.Init, 
                    Level = levelSnapshot, 
                    Login = request.Login};

                if (!loginResponses.TryAdd(request.Login, new ConcurrentQueue<ServerResponse>()))
                {
                    ProcessRejectLoginResponse(request);
                    return;
                }
                
                loginResponses[request.Login].Enqueue(initResponse);
                ProcessJoinResponses(request, newPlayer);
            }
            else
            {
                ProcessRejectLoginResponse(request);
            }
        }

        private void ProcessJoinResponses(LoginRequest request, AbstractPlayer newPlayer)
        {
            foreach (var targetLogin in loginResponses.Keys)
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
                loginResponses[targetLogin].Enqueue(response);
            }
        }

        private void ProcessRejectLoginResponse(LoginRequest request)
        {
            var rejectResponse = new ServerResponse
            {
                Type = ResponseType.LoginExists,
                Login = request.Login
            };
            loginResponses[request.Login].Enqueue(rejectResponse);
        }

        private void ProcessRequest(InputRequest request)
        {
            var key = KeyParser.ToConsoleKey(request.KeyInput);
            var login = request.Login;
            var character = level.GetCharacter(login);

            Console.WriteLine($"Receive {key.Key} {login}");

            foreach (var subscriber in subscribers)
            {
                subscriber.ProcessInput(key, character);
            }
        }

        public void MakeAction(AbstractPlayer player, ActionType actionType)
        {
            foreach (var targetLogin in responses.Keys)
            {
                var response = new ServerResponse
                {
                    Type = ResponseType.Action,
                    Login = player.Login,
                    KeyInput = KeyParser.FromActionTypeToKeyInput(actionType)
                };
                Console.WriteLine($"Sending {response.Type.ToString()} {response.KeyInput} to {targetLogin}");
                responses[targetLogin].Enqueue(response);
            }
        }

        public void MovePlayer(AbstractPlayer player, Position intentPosition)
        {
            foreach (var targetLogin in responses.Keys)
            {
                var response = new ServerResponse
                {
                    Type = ResponseType.Move,
                    Login = player.Login,
                    Pair = new Pair {X = intentPosition.X, Y = intentPosition.Y}
                };
                Console.WriteLine($"Sending {response.Type.ToString()} " +
                                  $"{intentPosition.Y} {intentPosition.X} to {targetLogin}");
                responses[targetLogin].Enqueue(response);
            }
        }

        public void Move(Mob mob, Position intentPosition)
        {
            foreach (var targetLogin in responses.Keys)
            {
                var response = new ServerResponse
                {
                    Type =  ResponseType.MobMove,
                    Login = mob.Id.ToString(),
                    Pair = new Pair {X = intentPosition.X, Y = intentPosition.Y}
                };
                Console.WriteLine($"Sending {response.Type.ToString()} {response.Pair.Y} " +
                                  $"{response.Pair.X} to {targetLogin}");
                responses[targetLogin].Enqueue(response);
            }
        }
    }
}