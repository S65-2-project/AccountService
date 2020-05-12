using System;

namespace AccountService.Exceptions
{
    [Serializable]
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException() : base("This email address is invalid.")
        {
            
        }
    }
}