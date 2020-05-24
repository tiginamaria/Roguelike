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
        private SaveGameInteractor saveGameInteractor;

        public EventHandler<AbstractPlayer> OnExit;

        public ExitGameInteractor()
        {
        }

        public ExitGameInteractor(Level level, IActionListener listener = null,
            SaveGameInteractor saveGameInteractor = null)
        {
            this.level = level;
            this.listener = listener;
            this.saveGameInteractor = saveGameInteractor;
        }

        public void SetLevel(Level level) => this.level = level;

        public void SetListener(IActionListener listener) => this.listener = listener;

        /// <summary>
        /// Deletes the character from the board.
        /// Saves the game if it is user controlled player and save interactor is set.
        /// Notifies listeners.
        /// </summary>
        public void Exit(Character character)
        {
            var player = character as AbstractPlayer;
            character.Delete(level.Board);
            level.DeletePlayer(player);

            if (level.IsCurrentPlayer(character))
            {
                saveGameInteractor?.Save(character);
                saveGameInteractor?.Dump();
                OnExit?.Invoke(this, player);
            }
            
            listener?.MakeAction(character as AbstractPlayer, ActionType.Exit);
        }
    }
}
