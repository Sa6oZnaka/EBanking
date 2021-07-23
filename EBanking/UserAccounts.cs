using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EBanking.Data.Entities;
using EBanking.Data;
using EBanking.Data.Interfaces;

namespace EBanking
{
    class UserAccounts
    {

        IEBankingDbContext _db;

        public List<UserAccount> All {
            get
            {
                return _db.UserAccounts.All.ToList();
            }
        }

        public decimal WithdrawFee {
            get
            {
                return 0.1m;
            } 
        }

        public UserAccounts(IEBankingDbContext db)
        {
            _db = db;
        }

        public bool sendToUser(Guid senderAccount, Guid receiverAccount, decimal amount)
        {
            if (Guid.Equals(senderAccount, receiverAccount))
                return false;

            if (userAccoutExist(senderAccount) && userAccoutExist(receiverAccount) && amount > 0)
            {
                // insufficient funds
                if (getUserBalance(senderAccount) < amount)
                    return false;

                updateBalance(senderAccount, -amount);
                updateBalance(receiverAccount, amount);

                Transaction txSender = new Transaction();
                txSender.UserAccountId = getUserAccountId(senderAccount);
                txSender.Key = Guid.NewGuid();
                txSender.Type = TransactionType.Debit;
                txSender.Amount = amount;
                txSender.EventDate = DateTime.Now;
                txSender.SystemComment = "Transaction from " + senderAccount + " to " + receiverAccount;

                Transaction txReceiver = new Transaction();
                txReceiver.UserAccountId = getUserAccountId(receiverAccount);
                txReceiver.Key = Guid.NewGuid();
                txReceiver.Type = TransactionType.Credit;
                txReceiver.Amount = amount;
                txReceiver.EventDate = DateTime.Now;
                txReceiver.SystemComment = "Transaction from " + senderAccount + " to " + receiverAccount;

                _db.Transactions.Insert(txSender);
                _db.Transactions.Insert(txReceiver);

                return true;
            }
            return false;
        }

        public bool deposit(Guid receiverAccount, decimal amount)
        {
            if (userAccoutExist(receiverAccount) && amount > 0)
            {
                updateBalance(receiverAccount, amount);
                
                Transaction txReceiver = new Transaction();
                txReceiver.UserAccountId = getUserAccountId(receiverAccount);
                txReceiver.Key = Guid.NewGuid();
                txReceiver.Type = TransactionType.Credit;
                txReceiver.Amount = amount;
                txReceiver.EventDate = DateTime.Now;
                txReceiver.SystemComment = "Deposit to " + receiverAccount;

                _db.Transactions.Insert(txReceiver);

                return true;
            }
            return false;
        }

        public bool withdraw(Guid senderAccount, decimal amount)
        {
            if (userAccoutExist(senderAccount) && amount > 0 && getUserBalance(senderAccount) >= Decimal.Add(amount, WithdrawFee))
            {
                updateBalance(senderAccount, -Decimal.Add(amount, WithdrawFee));

                Transaction txSender = new Transaction();
                txSender.UserAccountId = getUserAccountId(senderAccount);
                txSender.Key = Guid.NewGuid();
                txSender.Type = TransactionType.Debit;
                txSender.Amount = amount;
                txSender.EventDate = DateTime.Now;
                txSender.SystemComment = "Withdraw from " + senderAccount;

                Transaction txFee = new Transaction();
                txFee.UserAccountId = txSender.UserAccountId;
                txFee.Key = txSender.Key;
                txFee.Type = TransactionType.Debit;
                txFee.Amount = WithdrawFee;
                txFee.EventDate = DateTime.Now;
                txFee.SystemComment = "Transaction fee";

                _db.Transactions.Insert(txSender);
                _db.Transactions.Insert(txFee);

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

            All.Add(account);
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
            return All.Any(ua => ua.Key == key);
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
            return All.Find(ua => ua.Key == key);
        }

    }
}
