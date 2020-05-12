using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Roguelike.Network.Services
{
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
                Console.WriteLine($"Send login response {targetLogin} " +
                                  $"{loginResponse.Type.ToString()} to {targetLogin}");
                await responseStream.WriteAsync(loginResponse);
                if (loginResponse.Type == ResponseType.Init)
                {
                    Responses.TryAdd(targetLogin, new ConcurrentQueue<ServerResponse>());
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
            Requests.Enqueue(request);
            return Task.FromResult(new Empty());
        }
    }
}