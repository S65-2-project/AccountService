﻿using System;

namespace AccountService.Exceptions
{
    [Serializable]
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException()
            : base("An account with this id could not be found.")
        {
        }
    }
}