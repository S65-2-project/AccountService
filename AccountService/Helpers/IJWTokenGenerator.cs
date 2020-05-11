using System;

namespace AccountService.Helpers
{
    public interface IJWTokenGenerator
    {
        string GenerateJWT(Guid id);
    }
}