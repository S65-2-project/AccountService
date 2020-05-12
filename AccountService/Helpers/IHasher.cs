﻿using System.Threading.Tasks;

namespace AccountService.Helpers
{
    public interface IHasher
    {
        /// <summary>
        /// Hashes a given password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>byte[] hashed password</returns>
        Task<byte[]> HashPassword(string password, byte[] salt);

        /// <summary>
        /// Creates a Salt
        /// </summary>
        /// <returns>byte[] salt</returns>
        byte[] CreateSalt();

        /// <summary>
        /// verifies if a given string password matches a hashed password using salt. 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <returns>boolean</returns>
        Task<bool> VerifyHash(string password, byte[] salt, byte[] hash);
    }
}