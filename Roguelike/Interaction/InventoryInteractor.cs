using Roguelike.Model;
using Roguelike.View;

namespace Roguelike.Interaction
{
    public class InventoryInteractor
    {
        private IPlayView playView;
        private Level level;

        public InventoryInteractor(Level level, IPlayView playView)
        {
            this.level = level;
            this.playView = playView;
        }

        public void PutOn()
        {
            throw new System.NotImplementedException();
        }

        public void PutOff()
        {
            throw new System.NotImplementedException();
        }
    }
}