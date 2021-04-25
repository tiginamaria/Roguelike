using System;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using Roguelike.Initialization;
using Roguelike.Interaction;
using Roguelike.Model;
using Roguelike.Model.Mobs;
using Roguelike.Network;
using Roguelike.Network.Services;

namespace Roguelike.Input.Processors
{
    /// <summary>
    /// Receives an input from the server and pass it to interactors.
    /// Receives an input from the user and sends it to the server.
    /// </summary>
    public class ClientInputProcessor : IUpdatable, IInputProcessor
    {
        private readonly ClientService client;
        private readonly List<IInputProcessor> subscribers = new List<IInputProcessor>();
        
        private string login;
        private Level level;
        private MobMoveInteractor mobMoveInteractor;
        private PlayerMoveInteractor playerMoveInteractor;
        private SpawnPlayerInteractor spawnPlayerInteractor;

        private bool stopped;
        private int sessionId;

        public ClientInputProcessor(ClientService client) => this.client = client;

        public int CreateSession()
        {
            var response = client.CreateSession(new Empty());
            return response.Id;
        }
        
        public List<int> ListSessions() => client.ListSessions();

        public Level Login(string login, int sessionId)
        {
            this.sessionId = sessionId;
            this.login = login;
            client.Login(login, sessionId);
            level = ProcessInitResponse();

            return level;
        }
        
        public void AddInputProcessor(IInputProcessor inputProcessor) => 
            subscribers.Add(inputProcessor);

        public void SetMobInteractor(MobMoveInteractor mobMoveInteractor) => 
            this.mobMoveInteractor = mobMoveInteractor;

        public void SetPlayerMoveInteractor(PlayerMoveInteractor playerMoveInteractor) => 
            this.playerMoveInteractor = playerMoveInteractor;

        public void SetSpawnPlayerInteractor(SpawnPlayerInteractor spawnPlayerInteractor) => 
            this.spawnPlayerInteractor = spawnPlayerInteractor;

        private Level ProcessInitResponse()
        {
            while (true)
            {
                var initResponse = client.GetResponse();
                
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

        /// <summary>
        /// Lookup for the server response and pass it to the corresponding interactor.
        /// </summary>
        public void Update()
        {
            if (!client.TryGetResponse(out var serverResponse))
            {
                return;
            }

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
                    if (level.ContainsPlayer(incomingLogin))
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

        /// <summary>
        /// Stops receiving user input.
        /// </summary>
        public void Stop() => stopped = true;

        /// <summary>
        /// Pass the current input to the server.
        /// </summary>
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
                client.SendRequest(moveRequest);
            }
        }
    }
}