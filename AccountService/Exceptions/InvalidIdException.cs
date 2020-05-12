using System;

namespace AccountService.Exceptions
{
    [Serializable]
    public class InvalidIdException : Exception
    {
        public InvalidIdException() : base("This id is invalid.")
        {
            
        }
    }
}