using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Roguelike.Input.Processors;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.Mobs;
using Roguelike.Model.PlayerModel;
using Roguelike.Network;
using Roguelike.Network.Services;

namespace Roguelike.Input.Controllers
{
    public class ServerInputController : IMobMoveListener, IActionListener, IPlayerMoveListener, IUpdatable
    {
        private readonly Level level;
        private readonly int sessionId;
        private readonly List<IInputProcessor> subscribers = new List<IInputProcessor>();
        private readonly ServerInputService serverInputService;

        public ServerInputController(Level level, int sessionId, ServerInputService serverInputService)
        {
            this.level = level;
            this.sessionId = sessionId;
            this.serverInputService = serverInputService;
        }
        
        public void AddInputProcessor(IInputProcessor inputProcessor)
        {
            subscribers.Add(inputProcessor);
        }

        public void Update()
        {
            UpdateLoginRequest();
            UpdateInputRequest();
        }

        private void UpdateLoginRequest()
        {
            if (!serverInputService.LoginRequests.TryPeek(out var topRequest))
            {
                return;
            }

            if (topRequest.SessionId != sessionId)
            {
                return;
            }

            if (serverInputService.LoginRequests.TryDequeue(out var loginRequest))
            {
                ProcessLoginRequest(loginRequest);
            }
        }
        
        private void UpdateInputRequest()
        {
            if (!serverInputService.Requests.TryPeek(out var topRequest))
            {
                return;
            }

            if (topRequest.SessionId != sessionId)
            {
                return;
            }

            if (serverInputService.Requests.TryDequeue(out var loginRequest))
            {
                ProcessRequest(loginRequest);
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void ProcessLoginRequest(LoginRequest request)
        {
            if (!serverInputService.LoginResponses.ContainsKey(request.Login))
            {
                var newPlayer = level.AddPlayerAtEmpty(request.Login);
                var levelSnapshot = level.Save().ToString();
                
                var initResponse = new ServerResponse {
                    Type = ResponseType.Init, 
                    Level = levelSnapshot, 
                    Login = request.Login};

                if (!serverInputService.LoginResponses.TryAdd(request.Login, 
                    new ConcurrentQueue<ServerResponse>()))
                {
                    ProcessRejectLoginResponse(request);
                    return;
                }
                
                serverInputService.LoginResponses[request.Login].Enqueue(initResponse);
                ProcessJoinResponses(request, newPlayer);
            }
            else
            {
                ProcessRejectLoginResponse(request);
            }
        }

        private void ProcessJoinResponses(LoginRequest request, AbstractPlayer newPlayer)
        {
            foreach (var targetLogin in serverInputService.LoginResponses.Keys)
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
                serverInputService.LoginResponses[targetLogin].Enqueue(response);
            }
        }

        private void ProcessRejectLoginResponse(LoginRequest request)
        {
            var rejectResponse = new ServerResponse
            {
                Type = ResponseType.LoginExists,
                Login = request.Login
            };
            serverInputService.LoginResponses[request.Login].Enqueue(rejectResponse);
        }

        private void ProcessRequest(InputRequest request)
        {
            var key = KeyParser.ToConsoleKey(request.KeyInput);
            var login = request.Login;
            var character = level.GetPlayer(login);

            Console.WriteLine($"Receive {key.Key} {login}");

            foreach (var subscriber in subscribers)
            {
                subscriber.ProcessInput(key, character);
            }
        }

        public void MakeAction(AbstractPlayer player, ActionType actionType)
        {
            foreach (var targetLogin in serverInputService.Responses.Keys)
            {
                var response = new ServerResponse
                {
                    Type = ResponseType.Action,
                    Login = player.Login,
                    KeyInput = KeyParser.FromActionTypeToKeyInput(actionType)
                };
                Console.WriteLine($"Sending {response.Type.ToString()} {response.KeyInput} to {targetLogin}");
                serverInputService.Responses[targetLogin].Enqueue(response);
            }
        }

        public void MovePlayer(AbstractPlayer player, Position intentPosition)
        {
            foreach (var targetLogin in serverInputService.Responses.Keys)
            {
                var response = new ServerResponse
                {
                    Type = ResponseType.Move,
                    Login = player.Login,
                    Pair = new Pair {X = intentPosition.X, Y = intentPosition.Y}
                };
                Console.WriteLine($"Sending {response.Type.ToString()} " +
                                  $"{intentPosition.Y} {intentPosition.X} to {targetLogin}");
                serverInputService.Responses[targetLogin].Enqueue(response);
            }
        }

        public void Move(Mob mob, Position intentPosition)
        {
            foreach (var targetLogin in serverInputService.Responses.Keys)
            {
                var response = new ServerResponse
                {
                    Type =  ResponseType.MobMove,
                    Login = mob.Id.ToString(),
                    Pair = new Pair {X = intentPosition.X, Y = intentPosition.Y}
                };
                Console.WriteLine($"Sending {response.Type.ToString()} {response.Pair.Y} " +
                                  $"{response.Pair.X} to {targetLogin}");
                serverInputService.Responses[targetLogin].Enqueue(response);
            }
        }
    }
}