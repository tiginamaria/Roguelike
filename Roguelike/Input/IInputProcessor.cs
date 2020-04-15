using System;

namespace Roguelike.Input
{
    public interface IInputProcessor
    {
        void ProcessInput(ConsoleKey key);
    }
}