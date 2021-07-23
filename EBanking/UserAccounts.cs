using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EBankingDbContext = EBanking.Data.EBankingDbContext;
using UserAccount = EBanking.Data.Entities.UserAccount;

namespace EBanking
{
    class UserAccounts
    {

        EBankingDbContext _db;
        List<UserAccount> _userAccounts;
        Transactions _transactions;

        public UserAccounts(EBankingDbContext db)
        {
            _db = db;
            _userAccounts = _db.UserAccounts.All.ToList();

            _transactions = new Transactions(db);
        }

        public bool sendToUser(Guid senderAccount, Guid receiverAccount, decimal amount)
        {
            if (userAccoutExist(senderAccount) && userAccoutExist(receiverAccount) && amount > 0)
            {
                // insufficient funds
                if (getUserBalance(senderAccount) < amount)
                    return false;

                updateBalance(senderAccount, -amount);
                updateBalance(receiverAccount, amount);

                _transactions.add(senderAccount, receiverAccount, -amount, null, getUserAccountId(senderAccount));
                _transactions.add(receiverAccount, senderAccount, amount, null, getUserAccountId(receiverAccount));

                return true;
            }
            return false;
        }

        public bool deposit(Guid receiverAccount, decimal amount)
        {
            if (userAccoutExist(receiverAccount) && amount > 0)
            {
                updateBalance(receiverAccount, amount);
                _transactions.add(receiverAccount, null, amount, null, getUserAccountId(receiverAccount));
                return true;
            }
            return false;
        }

        public bool withdraw(Guid senderAccount, decimal amount)
        {
            if (userAccoutExist(senderAccount) && amount > 0 && getUserBalance(senderAccount) >= Decimal.Add(amount, _transactions.Tax))
            {
                updateBalance(senderAccount, -Decimal.Add(amount, _transactions.Tax));
                _transactions.add(senderAccount, null, -amount, null, getUserAccountId(senderAccount));
                return true;
            }
            return false;
        }

        public void add(string username, string userAccountName, Guid key, int userID)
        {
            UserAccount account = new UserAccount();
            account.UserId = userID;
            account.Key = key;
            account.FriendlyName = userAccountName;
            account.Balance = 0;

            _userAccounts.Add(account);
            _db.UserAccounts.Insert(account);
        }

        private bool updateBalance(Guid account, decimal amount)
        {
            if (!userAccoutExist(account)) return false;

            UserAccount userAccount = getUserAccount(account);
            userAccount.Balance = Decimal.Add(userAccount.Balance, amount);
            _db.UserAccounts.Update(userAccount);

            return true;
        }

        private bool userAccoutExist(Guid key)
        {
            return _userAccounts.Any(ua => ua.Key == key);
        }

        private int getUserAccountId(Guid key)
        {
            return getUserAccount(key).Id;
        }
        private decimal getUserBalance(Guid key)
        {
            return getUserAccount(key).Balance;
        }

        private UserAccount getUserAccount(Guid key)
        {
            return _userAccounts.Find(ua => ua.Key == key);
        }

    }
}
