using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Helpers
{
    public interface IJwtIdClaimReaderHelper
    {
        public Guid getUserIdFromToken(string jwt);
    }
}
