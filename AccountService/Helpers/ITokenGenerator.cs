using System;

namespace AccountService.Helpers
{
    public interface ITokenGenerator
    {
        string GenerateJwt(Guid id);
    }
}