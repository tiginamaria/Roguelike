using Roguelike.Model;
using Roguelike.View;

namespace Roguelike.Interaction
{
    /// <summary>
    /// Performs a logic of adding a player to the game.
    /// </summary>
    public class SpawnPlayerInteractor
    {
        private readonly IPlayView playView;
        private readonly Level level;

        public SpawnPlayerInteractor(Level level, IPlayView playView)
        {
            this.playView = playView;
            this.level = level;
        }
        
        /// <summary>
        /// Adds a player to the given position.
        /// </summary>
        public void Spawn(Position position, string login)
        {
            var player = level.RegisterPlayer(login, position);
            level.Board.SetObject(position, player);
            playView.UpdatePosition(level, position);
        }

        /// <summary>
        /// Removes a player with the given login.
        /// </summary>
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