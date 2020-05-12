using System;
using Roguelike.Input;
using Roguelike.Model;
using Roguelike.Model.PlayerModel;

namespace Roguelike.Interaction
{
    public class ExitGameInteractor
    {
        private readonly Level level;
        private readonly IActionListener listener;
        private readonly SaveGameInteractor saveGameInteractor;

        public EventHandler<AbstractPlayer> OnExit;

        public ExitGameInteractor(Level level, IActionListener listener = null,
            SaveGameInteractor saveGameInteractor = null)
        {
            this.level = level;
            this.listener = listener;
            this.saveGameInteractor = saveGameInteractor;
        }

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
