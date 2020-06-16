using AccountService.Domain;

namespace AccountService.Helpers
{
    public static class ExtenstionMethods
    {
        public static Account WithoutSensitiveData(this Account account) {
            account.Password = null;
            account.Salt = null;
            return account;
        }
    }
}