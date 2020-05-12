using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Roguelike.Initialization;

namespace Roguelike.Network
{
    public class SessionManager : SessionService.SessionServiceBase
    {
        private readonly ServerInputService inputService;
        private int id;
        private readonly Dictionary<int, IGameState> sessions = new Dictionary<int, IGameState>();

        public SessionManager(ServerInputService inputService)
        {
            this.inputService = inputService;
        }
        
        public override async Task<CreateSessionResponse> CreateSession(Empty request, ServerCallContext context)
        {
            var newSessionId = id;
            id++;
            
            var newSession = new ServerGameState(newSessionId, inputService);
            Console.WriteLine($"Creating a session {newSessionId}");
            Task.Run(() => newSession.InvokeState());
            sessions.Add(newSessionId, newSession);
            Console.WriteLine($"Session {newSessionId} created");
            
            return await Task.FromResult(new CreateSessionResponse {Id = newSessionId});
        }

        public override async Task ListSessions(Empty request, 
            IServerStreamWriter<ListSessionsResponse> responseStream, 
            ServerCallContext context)
        {
            foreach (var sessionsKey in sessions.Keys)
            {
                await responseStream.WriteAsync(new ListSessionsResponse {Id = sessionsKey});
            }
        }
    }
}