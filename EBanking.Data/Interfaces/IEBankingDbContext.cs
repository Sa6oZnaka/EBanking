using EBanking.Data.Entities;

namespace EBanking.Data.Interfaces
{
    public interface IEBankingDbContext : IDbContext
    {
        IEntitySet<int, User> Users { get; }
        IEntitySet<int, UserAccount> UserAccounts { get; }
        IEntitySet<int, Transaction> Transactions { get; }
    }
}
