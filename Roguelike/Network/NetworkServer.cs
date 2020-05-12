using Grpc.Core;
using Roguelike.Initialization;
using Roguelike.Network.Services;

namespace Roguelike.Network
{
    public class NetworkServer : IGameState
    {
        private readonly ServerInputService inputService;
        private readonly string host;
        private readonly int port;
        private readonly SessionService sessionService;

        public NetworkServer(string host = "localhost", int port = 8080)
        {
            inputService = new ServerInputService();
            sessionService = new SessionService(inputService);
            this.host = host;
            this.port = port;
        }

        public void InvokeState()
        {
            var server = new Server
            {
                Services = {
                    NetworkServerInputService.BindService(inputService), 
                    NetworkSessionService.BindService(sessionService)
                },
                Ports = {new ServerPort(host, port, ServerCredentials.Insecure)}
            };
            server.Start();

            // To prevent the server from stop
            while (true)
            {
            }
        }
    }
}