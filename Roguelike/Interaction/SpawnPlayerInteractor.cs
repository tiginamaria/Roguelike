using Roguelike.Model;
using Roguelike.View;

namespace Roguelike.Interaction
{
    public class SpawnPlayerInteractor
    {
        private readonly IPlayView playView;
        private readonly Level level;

        public SpawnPlayerInteractor(Level level, IPlayView playView)
        {
            this.playView = playView;
            this.level = level;
        }
        
        public void Spawn(Position position, string login)
        {
            var player = level.RegisterPlayer(login, position);
            level.Board.SetObject(position, player);
            playView.UpdatePosition(level, position);
        }

        public void DeletePlayer(string login)
        {
            if (!level.ContainsPlayer(login))
            {
                return;
            }
            
            var player = level.GetPlayer(login);
            player.Delete(level.Board);
            level.DeletePlayer(player);
            
            playView.UpdatePosition(level, player.Position);
        }
    }
}