using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Roguelike.Initialization;

namespace Roguelike.Network.Services
{
    /// <summary>
    /// Stores game sessions and provides an interface to them.
    /// </summary>
    public class SessionService : NetworkSessionService.NetworkSessionServiceBase
    {
        private readonly ServerInputService inputService;
        private int id;
        private readonly Dictionary<int, IGameState> sessions = new Dictionary<int, IGameState>();

        public SessionService(ServerInputService inputService) => this.inputService = inputService;

        /// <summary>
        /// Creates a game session and returns its new id asynchronously.
        /// </summary>
        public override async Task<CreateSessionResponse> CreateSession(Empty request, ServerCallContext context)
        {
            var newSessionId = id;
            id++;
            
            var newSession = new ServerGameState(newSessionId, inputService);
            Task.Run(() => newSession.InvokeState());
            sessions.Add(newSessionId, newSession);
            
            return await Task.FromResult(new CreateSessionResponse {Id = newSessionId});
        }

        /// <summary>
        /// Writes available session ids to the given stream asynchronously.
        /// </summary>
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