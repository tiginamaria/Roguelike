using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Roguelike.Initialization;
using Roguelike.Input.Processors;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Network;

namespace Roguelike.Input.Controllers
{
    public class ClientInputController : IUpdatable, IInputProcessor
    {
        private readonly ServerInputControllerService.ServerInputControllerServiceClient client;
        private IAsyncStreamReader<ServerResponse> call;
        private string login;
        private readonly List<IInputProcessor> subscribers = new List<IInputProcessor>();
        private Task<bool> checkIncomingTask;
        private Level level;
        private MobMoveInteractor mobMoveInteractor;

        public ClientInputController(string host = "localhost", int port = 8080)
        {
            var channel = new Channel($"{host}:{port}", ChannelCredentials.Insecure);
            client = new ServerInputControllerService.ServerInputControllerServiceClient(channel);
        }

        public Level Login(string login)
        {
            this.login = login;
            call = client.Login(new LoginRequest {Login = login}).ResponseStream;
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
                    return levelFactory.CreateLevel();
                }
            }
        }

        public void Update()
        {
            if (checkIncomingTask == null)
            {
                checkIncomingTask = call.MoveNext();
            }

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
            if (serverResponse.Type == ResponseType.Move)
            {
                var key = KeyParser.ToConsoleKey(serverResponse.KeyInput);
                var incomingLogin = serverResponse.Login;
                foreach (var subscriber in subscribers)
                {
                    subscriber.ProcessInput(key, level.GetCharacter(incomingLogin));
                }
            } 
            else if (serverResponse.Type == ResponseType.MobMove)
            {
                var incomingLogin = serverResponse.Login;
                var deltaMove = serverResponse.Pair;
                mobMoveInteractor.IntentMove(level.GetMob(incomingLogin), deltaMove.Y, deltaMove.X);
            }
            else if (serverResponse.Type == ResponseType.PlayerJoin)
            {
                level.AddPlayer(serverResponse.Login, 
                    new Position(serverResponse.Pair.Y, serverResponse.Pair.X));
            }
        }

        public void ProcessInput(ConsoleKeyInfo key, Character character)
        {
            var moveRequest = new InputRequest {Login = login, KeyInput = KeyParser.FromConsoleKey(key)};
            if (moveRequest.KeyInput != KeyInput.None)
            {
                client.MoveAsync(moveRequest);
            }
        }
    }
}