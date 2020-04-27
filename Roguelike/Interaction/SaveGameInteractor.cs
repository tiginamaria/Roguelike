using System.IO;
using Roguelike.Model;

namespace Roguelike.Interaction
{
    public class SaveGameInteractor
    {
        public const string SaveFileName = "dump.txt";
        private readonly Level level;
        private LevelSnapshot snapshot;

        public SaveGameInteractor(Level level)
        {
            this.level = level;
        }
        
        public void Save()
        {
            snapshot = level.Save();
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