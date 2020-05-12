using System;

namespace AccountService.Exceptions
{
    [Serializable]
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException()
            : base("A user with this email already exists.")
        {
        }
    }
}
