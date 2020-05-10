using Grpc.Core;
using Roguelike.Input.Controllers;

namespace Roguelike.Network
{
    public class NetworkServer
    {
        private readonly ServerInputController inputController;
        private const int Port = 8080;
        
        public NetworkServer(ServerInputController inputController)
        {
            this.inputController = inputController;
        }

        public void Start()
        {
            var server = new Server
            {
                Services = {ServerInputControllerService.BindService(inputController)},
                Ports = {new ServerPort("localhost", Port, ServerCredentials.Insecure)}
            };
            server.Start();
            server.ShutdownAsync().Wait();
        }
    }
}