using System;
using System.Threading;

namespace Roguelike
{
    public class GameLoop
    {
        private StateManager stateManager;
        private bool stopped;
        
        public GameLoop()
        {
            stateManager = StateManager.GetInstance();
        }
        
        public void Start(IGameState startState)
        {
            stopped = false;
            stateManager.ChangeState(startState);
            
            // TODO: timer handler
            while (!stopped)
            {
                stateManager.CurrentState.Update();
                stateManager.CurrentState.Draw();
            }
        }

        public void Stop()
        {
            stopped = true;
        }
    }
}