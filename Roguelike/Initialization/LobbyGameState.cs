using System;
using System.Collections.Generic;
using Roguelike.Exceptions;
using Roguelike.Input.Processors;

namespace Roguelike.Initialization
{
    public class LobbyGameState : IGameState
    {
        private readonly StateManager stateManager;
        private readonly ClientInputProcessor inputProcessor = new ClientInputProcessor();

        public LobbyGameState()
        {
            stateManager = StateManager.GetInstance();
        }
        
        public void InvokeState()
        {
            while (true)
            {
                var sessions = ListSessions();
                var id = ChooseSession(sessions);

                Console.Write("Input login: ");
                var login = Console.ReadLine();

                try
                {
                    var clientGameState = new ClientGameState(inputProcessor, login, id);
                    stateManager.ChangeState(clientGameState);
                }
                catch (LoginExistsException e)
                {
                    Console.WriteLine("Login exists, try again.");
                    Console.Error.WriteLine(e);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong, exiting...");
                    Console.Error.WriteLine(e);
                    break;
                }
            }
        }

        private int ChooseSession(ICollection<int> sessions)
        {
            if (sessions.Count == 0)
            {
                Console.WriteLine("No sessions available, creating a new one...");
                return inputProcessor.CreateSession();
            }
            
            int id;
            while (true)
            {
                Console.Write("Input session id: ");
                if (int.TryParse(Console.ReadLine(), out var sessionId))
                {
                    if (!sessions.Contains(sessionId))
                    {
                        Console.WriteLine("No such session, creating a new one...");
                        id = inputProcessor.CreateSession();
                    }
                    else
                    {
                        id = sessionId;
                    }

                    break;
                }

                Console.WriteLine("Must be an integer!");
            }

            return id;
        }

        private List<int> ListSessions()
        {
            var sessions = inputProcessor.ListSessions();
            Console.WriteLine($"Available sessions: {sessions.Count} found");
            foreach (var session in sessions)
            {
                Console.WriteLine($"* {session}");
            }

            Console.WriteLine();
            return sessions;
        }
    }
}
