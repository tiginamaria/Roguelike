using System;
using System.IO;
using Roguelike.Model;

namespace Roguelike.Interaction
{
    public class SaveGameInteractor
    {
        private readonly Level level;
        private LevelSnapshot snapshot;

        public SaveGameInteractor(Level level)
        {
            this.level = level;
        }
        
        public static string SaveFileName => 
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                "dump.txt");
        
        public void Save(Character character)
        {
            if (level.IsCurrentPlayer(character))
            {
                snapshot = level.Save();
            }
        }

        public void Dump()
        {
            snapshot.Dump(SaveFileName);
        }

        public void Delete()
        {
            if (File.Exists(SaveFileName))
            {
                File.Delete(SaveFileName);
            }
        }
    }
}