using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Roguelike.Network.Services
{
    public class ServerInputService : NetworkServerInputService.NetworkServerInputServiceBase
    {
        private readonly ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>> responses = 
            new ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>>();
        private readonly ConcurrentQueue<InputRequest> requests = new ConcurrentQueue<InputRequest>();
        private readonly ConcurrentQueue<LoginRequest> loginRequests = new ConcurrentQueue<LoginRequest>();
        private readonly ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>> loginResponses = 
            new ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>>();

        public ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>> LoginResponses => loginResponses;
        public ConcurrentDictionary<string, ConcurrentQueue<ServerResponse>> Responses => responses;
        public ConcurrentQueue<LoginRequest> LoginRequests => loginRequests;
        public ConcurrentQueue<InputRequest> Requests => requests;

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
                            loginResponses.TryRemove(request.Login, out _);
                            responses.TryRemove(request.Login, out _);
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
            Console.WriteLine($"Receive request {request.SessionId} {request.KeyInput} from {request.Login}");
            requests.Enqueue(request);
            return Task.FromResult(new Empty());
        }
    }
}