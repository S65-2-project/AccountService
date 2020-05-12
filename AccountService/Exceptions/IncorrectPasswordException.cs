using System;

namespace AccountService.Exceptions
{
    [Serializable]
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException()
            : base("This password is incorrect.")
        {
        }
    }
}