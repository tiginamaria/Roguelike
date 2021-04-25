using System;
using Roguelike.Interaction;

namespace Roguelike.Network
{
    /// <summary>
    /// Translates the input keys between different representations.
    /// </summary>
    public static class KeyParser
    {
        public static ConsoleKeyInfo ToConsoleKey(KeyInput requestKeyInput)
        {
            return requestKeyInput switch
            {
                KeyInput.A => new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false),
                KeyInput.F => new ConsoleKeyInfo('f', ConsoleKey.F, false, false, false),
                KeyInput.H => new ConsoleKeyInfo('h', ConsoleKey.H, false, false, false),
                KeyInput.E => new ConsoleKeyInfo('e', ConsoleKey.E, false, false, false),
                KeyInput.AltA => new ConsoleKeyInfo('a', ConsoleKey.A, true, false, false),
                KeyInput.AltF => new ConsoleKeyInfo('f', ConsoleKey.F, true, false, false),
                KeyInput.AltH => new ConsoleKeyInfo('h', ConsoleKey.H, true, false, false),
                KeyInput.AltE => new ConsoleKeyInfo('e', ConsoleKey.E, true, false, false),
                KeyInput.Up => new ConsoleKeyInfo('\0', ConsoleKey.UpArrow, true, false, false),
                KeyInput.Down => new ConsoleKeyInfo('\0', ConsoleKey.DownArrow, false, false, false),
                KeyInput.Left => new ConsoleKeyInfo('\0', ConsoleKey.LeftArrow, false, false, false),
                KeyInput.Right => new ConsoleKeyInfo('\0', ConsoleKey.RightArrow, false, false, false),
                KeyInput.None => new ConsoleKeyInfo('\0', ConsoleKey.NoName, false, false, false),
                KeyInput.Exit => new ConsoleKeyInfo('\0', ConsoleKey.Escape, false, false, false),
                _ => new ConsoleKeyInfo('\0', ConsoleKey.NoName, false, false, false),
            };
        }

        public static KeyInput FromConsoleKey(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.A when key.Modifiers == ConsoleModifiers.Shift:
                    return KeyInput.AltA;
                case ConsoleKey.F when key.Modifiers == ConsoleModifiers.Shift:
                    return KeyInput.AltF;
                case ConsoleKey.H when key.Modifiers == ConsoleModifiers.Shift:
                    return KeyInput.AltH;
                case ConsoleKey.E when key.Modifiers == ConsoleModifiers.Shift:
                    return KeyInput.AltE;
            }

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    return KeyInput.Up;
                case ConsoleKey.DownArrow:
                    return KeyInput.Down;
                case ConsoleKey.LeftArrow:
                    return KeyInput.Left;
                case ConsoleKey.RightArrow:
                    return KeyInput.Right;
                case ConsoleKey.A:
                    return KeyInput.A;
                case ConsoleKey.F:
                    return KeyInput.F;
                case ConsoleKey.H:
                    return KeyInput.H;
                case ConsoleKey.E:
                    return KeyInput.E;
                case ConsoleKey.Escape:
                    return KeyInput.Exit;
            }

            return KeyInput.None;
        }

        public static KeyInput FromActionTypeToKeyInput(ActionType actionType)
        {
            return actionType switch
            {
                ActionType.Exit => KeyInput.Exit,
                ActionType.ApplyAll => KeyInput.A,
                ActionType.ApplyForce => KeyInput.F,
                ActionType.ApplyHealth => KeyInput.H,
                ActionType.ApplyExperience => KeyInput.E,
                ActionType.TakeOffAll => KeyInput.AltA,
                ActionType.TakeOffForce => KeyInput.AltF,
                ActionType.TakeOffHealth => KeyInput.AltH,
                ActionType.TakeOffExperience => KeyInput.E,
                _ => KeyInput.None
            };
        }
    }
}