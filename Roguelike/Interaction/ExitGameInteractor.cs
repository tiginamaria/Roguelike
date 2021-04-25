using System;
using Roguelike.Input;
using Roguelike.Model;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Interaction
{
    /// <summary>
    /// Performs a logic of exiting the game.
    /// </summary>
    public class ExitGameInteractor
    {
        private Level level;
        private IActionListener listener;

        public EventHandler<AbstractPlayer> OnExit;

        public ExitGameInteractor()
        {
        }

        public ExitGameInteractor(Level level, IActionListener listener = null)
        {
            this.level = level;
            this.listener = listener;
        }

        public void SetLevel(Level level) => this.level = level;

        public void SetListener(IActionListener listener) => this.listener = listener;

        /// <summary>
        /// Deletes the character from the board.
        /// Notifies listeners.
        /// </summary>
        public void Exit(Character character)
        {
            var player = character as AbstractPlayer;
            if (level.IsCurrentPlayer(character))
            {
                character.Delete(level.Board);
                level.DeletePlayer(player);
                OnExit?.Invoke(this, player);
            }
            
            listener?.MakeAction(character as AbstractPlayer, ActionType.Exit);
        }
    }
}
