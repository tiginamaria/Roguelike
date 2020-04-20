using System;

namespace Roguelike.Input.Processors
{
    public interface IInputProcessor
    {
        void ProcessInput(ConsoleKey key);
    }
}
