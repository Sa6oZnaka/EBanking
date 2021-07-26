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
    public class UserAccounts
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

        public void sendToUser(Guid senderAccount, Guid receiverAccount, decimal amount)
        {
            if (Guid.Equals(senderAccount, receiverAccount))
                throw new Exception("Can't send to the same account!");
            if (!userAccoutExist(senderAccount))
                throw new Exception("Sender account doesn't exist!");
            if (!userAccoutExist(receiverAccount))
                throw new Exception("Receiver account doesn't exist!");
            if(amount <= 0)
                throw new Exception("Amount must be more than 0!");            
            if (getUserBalance(senderAccount) < amount)
                throw new Exception("Insufficient funds!");

            using (var transaction = _db.StartDbTransaction())
            {
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

                transaction.Commit();
            }
        }

        public void deposit(Guid receiverAccount, decimal amount)
        {
            if (!userAccoutExist(receiverAccount))
                throw new Exception("Receiver account doesn't exist!");
            if (amount <= 0)
                throw new Exception("Amount must be more than 0!");

            using (var transaction = _db.StartDbTransaction())
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
                transaction.Commit();
            }
        }

        public void withdraw(Guid senderAccount, decimal amount)
        {
            if (!userAccoutExist(senderAccount))
                throw new Exception("Sender account doesn't exist!");
            if (amount <= 0)
                throw new Exception("Amount must be more than 0!");
            if (getUserBalance(senderAccount) < Decimal.Add(amount, WithdrawFee))
                throw new Exception("Insufficient funds!");

            using (var transaction = _db.StartDbTransaction())
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

                transaction.Commit();
            }
        }

        public void add(string userAccountName, int userID)
        {
            using (var transaction = _db.StartDbTransaction())
            {
                UserAccount account = new UserAccount();
                account.UserId = userID;
                account.FriendlyName = userAccountName;
                account.Balance = 0;

                account.Key = Guid.NewGuid();
                while (userAccoutExist(account.Key))
                    account.Key = Guid.NewGuid();

                All.Add(account);
                _db.UserAccounts.Insert(account);

                transaction.Commit();
            }
        }

        private void updateBalance(Guid account, decimal amount)
        {
            if (!userAccoutExist(account))
                throw new Exception("User doesn't exist!");

            UserAccount userAccount = getUserAccount(account);
            userAccount.Balance = Decimal.Add(userAccount.Balance, amount);
            _db.UserAccounts.Update(userAccount);
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
