using Grpc.Core;
using Roguelike.Input.Controllers;

namespace Roguelike.Network
{
    public class NetworkServer
    {
        private readonly ServerInputController inputController;
        
        public NetworkServer(ServerInputController inputController)
        {
            this.inputController = inputController;
        }

        public void Start(string host = "localhost", int port = 8080)
        {
            var server = new Server
            {
                Services = {ServerInputControllerService.BindService(inputController)},
                Ports = {new ServerPort(host, port, ServerCredentials.Insecure)}
            };
            server.Start();
        }
    }
}