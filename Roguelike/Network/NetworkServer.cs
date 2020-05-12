using Grpc.Core;
using Roguelike.Initialization;

namespace Roguelike.Network
{
    public class NetworkServer : IGameState
    {
        private readonly ServerInputService inputService;
        private readonly string host;
        private readonly int port;
        private readonly SessionManager sessionManager;

        public NetworkServer(string host = "localhost", int port = 8080)
        {
            inputService = new ServerInputService();
            sessionManager = new SessionManager(inputService);
            this.host = host;
            this.port = port;
        }

        public void InvokeState()
        {
            var server = new Server
            {
                Services = {
                    ServerInputControllerService.BindService(inputService), 
                    SessionService.BindService(sessionManager)
                },
                Ports = {new ServerPort(host, port, ServerCredentials.Insecure)}
            };
            server.Start();

            while (true)
            {
            }
        }
    }
}