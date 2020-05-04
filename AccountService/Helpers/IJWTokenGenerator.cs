using System;
using AccountService.Domain;

namespace AccountService.Helpers
{
    public interface IJWTokenGenerator
    {
        string GenerateJWT(Guid id);
    }
}