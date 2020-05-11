using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Roguelike.Initialization;
using Roguelike.Input.Controllers;
using Roguelike.Input.Processors;
using Roguelike.Model;

namespace Roguelike.Network
{
    public class NetworkClient : IUpdatable, IInputProcessor
    {
        private readonly ServerInputControllerService.ServerInputControllerServiceClient client;
        private AsyncServerStreamingCall<ServerResponse> call;
        private string login;
        private readonly List<IInputProcessor> subscribers = new List<IInputProcessor>();
        private Task<bool> checkIncomingTask;
        
        public NetworkClient(string host = "localhost", int port = 8080)
        {
            var channel = new Channel($"{host}:{port}", ChannelCredentials.Insecure);
            client = new ServerInputControllerService.ServerInputControllerServiceClient(channel);
        }

        public Level Login(string login)
        {
            this.login = login;
            call = client.Login(new LoginRequest {Login = login});
            
            return ProcessInitResponse();
        }
        
        public void AddInputProcessor(IInputProcessor inputProcessor)
        {
            subscribers.Add(inputProcessor);
        }

        private Level ProcessInitResponse()
        {
            while (true)
            {
                var initTask = call.ResponseStream.MoveNext();
                initTask.Wait();
                var initResponse = call.ResponseStream.Current;
                
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
            checkIncomingTask ??= call.ResponseStream.MoveNext();
            
            if (!checkIncomingTask.IsCompleted)
            {
                return;
            }

            if (!checkIncomingTask.Result)
            {
                return;
            }

            checkIncomingTask = null;
            var serverResponse = call.ResponseStream.Current;
            if (serverResponse.Type == ResponseType.Move)
            {
                // TODO: different users
                var key = KeyParser.ToConsoleKey(serverResponse.KeyInput);
                foreach (var subscriber in subscribers)
                {
                    subscriber.ProcessInput(key);
                }
            }
        }

        public void ProcessInput(ConsoleKeyInfo key)
        {
            var moveRequest = GetRequest(key);
            if (moveRequest.KeyInput != KeyInput.None)
            {
                client.MoveAsync(moveRequest);
            }
        }

        private InputRequest GetRequest(ConsoleKeyInfo key)
        {
            return new InputRequest {Login = login, KeyInput = KeyParser.FromConsoleKey(key)};
        }
    }
}