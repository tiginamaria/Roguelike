namespace Roguelike.Model
{
    public interface ICharacter
    {
        CharacterStatistics GetStatistics();
        void Confuse(ICharacter other);
        void AcceptConfuse(ICharacter other);
    }
}