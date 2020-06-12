using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Exceptions
{
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException() : base("You are not authorised to do this action") { }
    }
}
