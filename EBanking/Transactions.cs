using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EBankingDbContext = EBanking.Data.EBankingDbContext;
using Transaction = EBanking.Data.Entities.Transaction;
using TransactionType = EBanking.Data.Entities.TransactionType;

namespace EBanking
{
    class Transactions
    {

        decimal _tax;
        List<Transaction> _transactions;
        EBankingDbContext _db;

        public decimal Tax {
            get
            {
                return _tax;
            }
        }

        public Transactions(EBankingDbContext db)
        {
            _db = db;

            _tax = 0.1m;
            _transactions = _db.Transactions.All.ToList();
        }

        public void add(Guid myAccount, Guid? otherAccount, decimal amount, Guid? key, int userAccountId)
        {
            Transaction tx = new Transaction();
            tx.Id = _transactions.Count;
            tx.UserAccountId = userAccountId;
            tx.Amount = amount;
            tx.EventDate = DateTime.Now;
            // check if transaction key is provided (for fee)
            if (key.HasValue)
                tx.Key = key.Value;
            else
                tx.Key = Guid.NewGuid();

            if (amount > 0)
            {
                tx.Type = TransactionType.Credit;
                if (otherAccount.HasValue)
                    tx.SystemComment = "Transaction from " + otherAccount + " to " + myAccount;
                else
                    tx.SystemComment = "Deposit to " + myAccount;
            }
            else
            {
                tx.Type = TransactionType.Debit;
                if (otherAccount.HasValue)
                    tx.SystemComment = "Transaction from " + myAccount + " to " + otherAccount;
                else
                {
                    tx.SystemComment = "Withdraw from " + myAccount;
                    // Add new transaction for fee
                    if (!key.HasValue)
                        add(myAccount, null, -_tax, tx.Key, userAccountId);
                }
            }

            _db.Transactions.Insert(tx);
            _transactions.Add(tx);
        }

    }
}
