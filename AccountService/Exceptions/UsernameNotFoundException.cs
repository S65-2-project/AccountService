﻿using System;

namespace AccountService.Exceptions
{
    [Serializable]
    public class UsernameNotFoundException : Exception
    {
        public UsernameNotFoundException()
            : base("A user with this username was not found.")
        {
        }
    }
}
