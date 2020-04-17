using System.Threading.Tasks;
using AccountService.Domain;
using AccountService.Models;

namespace AccountService.Services
{
    public interface IAccService
    {
        Task<Account> CreateAccount(string email, string password);
        Task<Account> UpdateAccount(string email, string password);
        Task<Account> UpdateRole(string email, bool isDelegate, bool isDAppOwner);
        Task<Account> GetAccount(string email);
        Task DeleteAccount(string email);
    }
}