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
        
        public void Start()
        {
            stopped = false;
            stateManager.ChangeState(StateType.Play);
            
            // TODO: timer handler
            while (!stopped)
            {
                stateManager.CurrentState.Update();
                
                Thread.Sleep(TimeSpan.FromMilliseconds(10));
            }
        }

        public void Stop()
        {
            stopped = true;
        }
    }
}