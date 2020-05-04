using System;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Models;

namespace AccountService.Services
{
    public interface IAccService
    {
        /// <summary>
        /// Creates an account 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Account> CreateAccount(CreateAccountModel model);
        
        /// <summary>
        /// Logs a user in if specified data is correct. Returns a JWToken
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        Task<Account> Login(LoginModel loginModel);

        /// <summary>
        /// Updates Role and email if applicable
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Account> UpdateAccount(Guid id, UpdateAccountModel model);
        
        /// <summary>
        /// Updates the password of an account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Account> UpdatePassword(Guid id, UpdateAccountModel model);
        
        /// <summary>
        /// Returns an account that has this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Account> GetAccount(Guid id);
        
        /// <summary>
        /// Removes an account
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task DeleteAccount(Guid id);
    }
}