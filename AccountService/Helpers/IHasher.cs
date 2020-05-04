﻿using System.Threading.Tasks;

namespace AccountService.Helpers
{
    public interface IHasher
    {
        Task<byte[]> HashPassword(string password, byte[] salt);

        byte[] CreateSalt();

        Task<bool> VerifyHash(string password, byte[] salt, byte[] hash);
    }
}