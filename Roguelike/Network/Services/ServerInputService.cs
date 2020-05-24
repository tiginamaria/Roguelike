using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Roguelike.Network.Services
{
    /// <summary>
    /// Listen for the client to login and interacts with it.
    /// Flattens the multithreading of incoming messages in one queue.
    /// </summary>
    public class ServerInputService : NetworkServerInputService.NetworkServerInputServiceBase
    {
        public ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>> LoginResponses { get; } = 
            new ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>>();
        public ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>> Responses { get; } = 
            new ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>>();
        public ConcurrentQueue<LoginRequest> LoginRequests { get; } = 
            new ConcurrentQueue<LoginRequest>();
        public ConcurrentQueue<InputRequest> Requests { get; } = 
            new ConcurrentQueue<InputRequest>();

        /// <summary>
        /// Starts a session with a client.
        /// </summary>
        public override async Task Login(LoginRequest request, 
            IServerStreamWriter<ServerResponse> responseStream,
            ServerCallContext context)
        {
            try
            {
                LoginRequests.Enqueue(request);

                while (true)
                {
                    if (!LoginResponses.ContainsKey(request.Login))
                    {
                        continue;
                    }
                    
                    if (LoginResponses[request.Login].TryDequeue(out var loginResponse))
                    {
                        await SendLoginResponses(request.Login, responseStream, loginResponse);
                    }

                    if (!Responses.ContainsKey(request.Login))
                    {
                        continue;
                    }

                    if (Responses[request.Login].TryDequeue(out var response))
                    {
                        if (response.KeyInput == KeyInput.Exit && response.Login == request.Login)
                        {
                            LoginResponses.TryRemove(request.Login, out _);
                            Responses.TryRemove(request.Login, out _);
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
            IAsyncStreamWriter<ServerResponse> responseStream,
            ServerResponse loginResponse)
        {
            if (loginResponse.Type == ResponseType.Init || loginResponse.Type == ResponseType.LoginExists)
            {
                await responseStream.WriteAsync(loginResponse);
                if (loginResponse.Type == ResponseType.Init)
                {
                    Responses.TryAdd(targetLogin, new ConcurrentQueue<ServerResponse>());
                }
            }
            else if (loginResponse.Type == ResponseType.PlayerJoin)
            {
                await responseStream.WriteAsync(loginResponse);
            }
        }

        /// <summary>
        /// Adds the incoming move request to the queue.
        /// </summary>
        public override Task<Empty> Move(InputRequest request, ServerCallContext context)
        {
            Requests.Enqueue(request);
            return Task.FromResult(new Empty());
        }
    }
}