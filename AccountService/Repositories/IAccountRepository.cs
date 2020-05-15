using System;
using System.Threading.Tasks;
using AccountService.Domain;

namespace AccountService.Repositories
{
    public interface IAccountRepository
    {
        /// <summary>
        /// Get Account by email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Account</returns>
        Task<Account> Get(string email);
        
        /// <summary>
        /// Get Account by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Account</returns>
        Task<Account> Get(Guid id);
        
        /// <summary>
        /// Creates an Account with the given account data
        /// </summary>
        /// <param name="account"></param>
        /// <returns>Account</returns>
        Task<Account> Create(Account account);
        
        /// <summary>
        /// Updates any changes to the saved account data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <returns>Account</returns>
        Task<Account> Update(Guid id, Account account);
        
        /// <summary>
        /// Removes an account using the specified ID
        /// </summary>
        /// <param name="id"></param>
        Task Remove(Guid id);
    }
}