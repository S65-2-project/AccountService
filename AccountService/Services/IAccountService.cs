using System;
using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Models;

namespace AccountService.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Creates an account 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Account</returns>
        Task<Account> CreateAccount(CreateAccountModel model);
        
        /// <summary>
        /// Logs a user in if specified data is correct.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>JWToken</returns>
        Task<Account> Login(LoginModel loginModel);

        /// <summary>
        /// Updates Role and email if applicable
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns>Account</returns>
        Task<Account> UpdateAccount(Guid id, UpdateAccountModel model);
        
        /// <summary>
        /// Updates the password of an account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns>Account</returns>
        Task<Account> UpdatePassword(Guid id, ChangePasswordModel passwordModel);
        
        /// <summary>
        /// Get an account that has this ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Account</returns>
        Task<Account> GetAccount(Guid id);
        
        /// <summary>
        /// Removes an account
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task DeleteAccount(Guid id);
    }
}