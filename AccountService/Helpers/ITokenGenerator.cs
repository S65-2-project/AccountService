using System;

namespace AccountService.Helpers
{
    public interface ITokenGenerator
    {
        /// <summary>
        /// Generates a JWT
        /// </summary>
        /// <param name="id"></param>
        /// <returns>string JWToken</returns>
        string GenerateJwt(Guid id);
    }
}