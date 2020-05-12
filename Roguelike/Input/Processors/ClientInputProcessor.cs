using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Roguelike.Initialization;
using Roguelike.Input.Controllers;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.Mobs;
using Roguelike.Network;

namespace Roguelike.Input.Processors
{
    public class ClientInputProcessor : IUpdatable, IInputProcessor
    {
        private IAsyncStreamReader<ServerResponse> call;
        private string login;
        private Task<bool> checkIncomingTask;
        private Level level;
        private MobMoveInteractor mobMoveInteractor;
        private PlayerMoveInteractor playerMoveInteractor;
        private SpawnPlayerInteractor spawnPlayerInteractor;
        private readonly List<IInputProcessor> subscribers = new List<IInputProcessor>();
        private readonly ServerInputControllerService.ServerInputControllerServiceClient client;
        private bool stopped;
        private readonly SessionService.SessionServiceClient sessionClient;
        private int sessionId;

        public ClientInputProcessor(string host = "localhost", int port = 8080)
        {
            var channel = new Channel($"{host}:{port}", ChannelCredentials.Insecure);
            client = new ServerInputControllerService.ServerInputControllerServiceClient(channel);
            sessionClient = new SessionService.SessionServiceClient(channel);
        }

        public int CreateSession()
        {
            var response = sessionClient.CreateSession(new Empty());
            return response.Id;
        }
        
        public List<int> ListSessions()
        {
            var result = new List<int>();
            var response = sessionClient.ListSessions(new Empty());
            while (true)
            {
                var task = response.ResponseStream.MoveNext();
                task.Wait();
                if (!task.Result)
                {
                    break;
                }
                result.Add(response.ResponseStream.Current.Id);
            }

            return result;
        }

        public Level Login(string login, int sessionId)
        {
            this.sessionId = sessionId;
            this.login = login;
            call = client.Login(new LoginRequest {Login = login, SessionId = sessionId}).ResponseStream;
            level = ProcessInitResponse();

            return level;
        }
        
        public void AddInputProcessor(IInputProcessor inputProcessor)
        {
            subscribers.Add(inputProcessor);
        }

        public void SetMobInteractor(MobMoveInteractor mobMoveInteractor)
        {
            this.mobMoveInteractor = mobMoveInteractor;
        }
        
        public void SetPlayerMoveInteractor(PlayerMoveInteractor playerMoveInteractor)
        {
            this.playerMoveInteractor = playerMoveInteractor;
        }
        
        public void SetSpawnPlayerInteractor(SpawnPlayerInteractor spawnPlayerInteractor)
        {
            this.spawnPlayerInteractor = spawnPlayerInteractor;
        }

        private Level ProcessInitResponse()
        {
            while (true)
            {
                var initTask = call.MoveNext();
                initTask.Wait();
                var initResponse = call.Current;

                if (initResponse.Type == ResponseType.LoginExists)
                {
                    return null;
                }

                if (initResponse.Type == ResponseType.Init)
                {
                    var snapshot = initResponse.Level;
                    var levelFactory = FileLevelFactory.FromString(snapshot);
                    levelFactory.SetMobFactory(new NetworkMobFactory());
                    return levelFactory.CreateLevel();
                }
            }
        }

        public void Update()
        {
            checkIncomingTask ??= call.MoveNext();

            if (!checkIncomingTask.IsCompleted)
            {
                return;
            }

            if (!checkIncomingTask.Result)
            {
                checkIncomingTask = null;
                return;
            }    
            
            checkIncomingTask = null;
            
            var serverResponse = call.Current;

            if (serverResponse.Type == ResponseType.Action)
            {
                var incomingLogin = serverResponse.Login;
                if (serverResponse.KeyInput == KeyInput.Exit)
                {
                    spawnPlayerInteractor.DeletePlayer(incomingLogin);
                    return;
                }
                
                var key = KeyParser.ToConsoleKey(serverResponse.KeyInput);
                foreach (var subscriber in subscribers)
                {
                    if (level.ContainsCharacter(incomingLogin))
                    {
                        subscriber.ProcessInput(key, level.GetPlayer(incomingLogin));
                    }
                }
            } 
            else if (serverResponse.Type == ResponseType.MobMove)
            {
                var incomingLogin = serverResponse.Login;
                var deltaMove = serverResponse.Pair;
                mobMoveInteractor.IntentMove(level.GetMob(incomingLogin), deltaMove.Y, deltaMove.X);
            }
            else if (serverResponse.Type == ResponseType.Move)
            {
                var incomingLogin = serverResponse.Login;
                var deltaMove = serverResponse.Pair;
                playerMoveInteractor.IntentMove(level.GetPlayer(incomingLogin), deltaMove.Y, deltaMove.X);
            }
            else if (serverResponse.Type == ResponseType.PlayerJoin)
            {
                var position = new Position(serverResponse.Pair.Y, serverResponse.Pair.X);
                spawnPlayerInteractor.Spawn(position, serverResponse.Login);
            }
        }

        public void Stop()
        {
            stopped = true;
        }

        public void ProcessInput(ConsoleKeyInfo key, Character character)
        {
            if (stopped)
            {
                return;
            }
            
            var moveRequest = new InputRequest
            {
                Login = login, 
                KeyInput = KeyParser.FromConsoleKey(key),
                SessionId = sessionId
            };
            
            if (moveRequest.KeyInput != KeyInput.None)
            {
                client.MoveAsync(moveRequest);
            }
        }
    }
}