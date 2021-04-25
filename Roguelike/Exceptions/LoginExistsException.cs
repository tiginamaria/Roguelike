using System;

namespace Roguelike.Exceptions
{
    /// <summary>
    /// Raised on attempt to enter a game with an already existed login.
    /// </summary>
    public class LoginExistsException : Exception
    {
    }
}