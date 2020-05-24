using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Roguelike.Network.Services
{
    /// <summary>
    /// A service to receive and send requests to the server 
    /// </summary>
    public class ClientService
    {
        private IAsyncStreamReader<ServerResponse> call;
        private readonly NetworkServerInputService.NetworkServerInputServiceClient client;
        private readonly NetworkSessionService.NetworkSessionServiceClient sessionClient;
        private Task<bool> checkIncomingTask;
        
        public ClientService(string host = "localhost", int port = 8080)
        {
            var channel = new Channel($"{host}:{port}", ChannelCredentials.Insecure);
            client = new NetworkServerInputService.NetworkServerInputServiceClient(channel);
            sessionClient = new NetworkSessionService.NetworkSessionServiceClient(channel);
        }

        public CreateSessionResponse CreateSession(Empty empty) => sessionClient.CreateSession(empty);

        /// <summary>
        /// Returns a list of available session ids.
        /// </summary>
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

        /// <summary>
        /// Logins to the given session with the given login.
        /// </summary>
        public void Login(string login, int sessionId) => 
            call = client.Login(new LoginRequest {Login = login, SessionId = sessionId}).ResponseStream;

        /// <summary>
        /// Synchronously waits for the response from the server.
        /// </summary>
        public ServerResponse GetResponse()
        {
            var initTask = call.MoveNext();
            initTask.Wait();
            return call.Current;
        }
        
        /// <summary>
        /// Returns a response from the server in non-blocking mode.
        /// Returns true if response was received and false otherwise.
        /// </summary>
        public bool TryGetResponse(out ServerResponse serverResponse)
        {
            checkIncomingTask ??= call.MoveNext();
            serverResponse = null;
            
            if (!checkIncomingTask.IsCompleted)
            {
                return false;
            }

            if (!checkIncomingTask.Result)
            {
                checkIncomingTask = null;
                return false;
            }

            checkIncomingTask = null;

            serverResponse = call.Current;
            return true;
        }

        /// <summary>
        /// Send a request in non-blocking mode.
        /// </summary>
        public void SendRequest(InputRequest moveRequest) => client.MoveAsync(moveRequest);
    }
}