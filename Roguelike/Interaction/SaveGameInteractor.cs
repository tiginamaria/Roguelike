using System;
using System.IO;
using Roguelike.Model;

namespace Roguelike.Interaction
{
    /// <summary>
    /// Performs a logic of saving game state.
    /// </summary>
    public class SaveGameInteractor
    {
        private readonly Level level;
        private LevelSnapshot snapshot;

        public SaveGameInteractor(Level level) => this.level = level;

        /// <summary>
        /// Returns a path to the file to save in.
        /// </summary>
        public static string SaveFileName => 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                "dump.txt");
        
        /// <summary>
        /// Saves the state into the local snapshot.
        /// </summary>
        public void Save(Character character)
        {
            if (level.IsCurrentPlayer(character))
            {
                snapshot = level.Save();
            }
        }

        /// <summary>
        /// Dumps the current snapshot on disk.
        /// </summary>
        public void Dump() => snapshot.Dump(SaveFileName);

        /// <summary>
        /// Deletes the file with a saved game if exists.
        /// </summary>
        public void DeleteSaving()
        {
            if (File.Exists(SaveFileName))
            {
                File.Delete(SaveFileName);
            }
        }
    }
}