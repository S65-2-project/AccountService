using System.Threading.Tasks;
using AccountService.Domain;

namespace AccountService.Services
{
    public interface IAccountService
    {
        Task CreateAccount(Account account);
        Task<Account> Get(Account account);
        Task DeleteAccount(Account account);
    }
}