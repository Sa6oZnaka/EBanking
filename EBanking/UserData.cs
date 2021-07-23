using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using User = EBanking.Data.Entities.User;
using UserAccount = EBanking.Data.Entities.UserAccount;
using Transaction = EBanking.Data.Entities.Transaction;
using TransactionType = EBanking.Data.Entities.TransactionType;
using EBankingDbContext = EBanking.Data.EBankingDbContext;

namespace EBanking
{
    class UserData
    {

        List<User> _users;
        List<UserAccount> _userAccounts;
        List<Transaction> _transactions;
        EBankingDbContext _db = new EBankingDbContext("./db.txt");
        decimal _tax;

        public UserData()
        {
            _users = _db.Users.All.ToList();
            _userAccounts = _db.UserAccounts.All.ToList();
            _transactions = _db.Transactions.All.ToList();

            _tax = 0.1m;
            // Add user
            //Console.WriteLine(addUser("Alex1", "12345", "Random Name", "test@test.test"));

            // Add user account
            //Console.WriteLine(addUserAccount("Alex2", "Savings", Guid.NewGuid()));

            // Deposit
            Console.WriteLine("Deposit " + deposit(Guid.Parse("76be7f2f-28d8-4395-a424-9abe7c536dc9"), 1000) );

            // Add transaction
            Console.WriteLine("TX " + sendToUser(Guid.Parse("76be7f2f-28d8-4395-a424-9abe7c536dc9"), Guid.Parse("8e94b850-5dfd-4223-9225-52e659c79495"), 15));

            // Withdraw
            Console.WriteLine("Withdraw " + withdraw(Guid.Parse("76be7f2f-28d8-4395-a424-9abe7c536dc9"), 200));

            Console.WriteLine("Auth:  " + authenticate("Alex1", "123456"));
            Console.WriteLine("Auth:  " + authenticate("Alex1", "12345"));
            Console.WriteLine("Users: " + _users.Count);
            Console.WriteLine("UsersAccounts: " + _userAccounts.Count);
        }

        bool authenticate(string username, string password)
        {
            return _users.Any(u => u.Username == username && u.Password == password);
        }

        bool sendToUser(Guid senderAccount, Guid receiverAccount, decimal amount)
        {
            if(userAccoutExist(senderAccount) && userAccoutExist(receiverAccount) && amount > 0)
            {
                // insufficient funds
                if (getUserBalance(senderAccount) < amount)
                    return false;
                
                updateBalance(senderAccount, -amount);
                updateBalance(receiverAccount, amount);

                addTransaction(senderAccount, receiverAccount, -amount, null);
                addTransaction(receiverAccount, senderAccount, amount, null);

                return true;
            }
            return false;
        }

        bool deposit(Guid receiverAccount, decimal amount) {
            if (userAccoutExist(receiverAccount) && amount > 0)
            {
                updateBalance(receiverAccount, amount);
                addTransaction(receiverAccount, null, amount, null);
                return true;
            }
            return false;
        }

        bool withdraw(Guid senderAccount, decimal amount)
        {
            if(userAccoutExist(senderAccount) && amount > 0 && getUserBalance(senderAccount) >= Decimal.Add(amount, _tax))
            {
                updateBalance(senderAccount, -Decimal.Add(amount,_tax));
                addTransaction(senderAccount, null, -amount, null);
                return true;
            }
            return false;
        }


        void addTransaction(Guid myAccount, Guid? otherAccount, decimal amount, Guid? key)
        {
            Transaction tx = new Transaction();
            tx.UserAccountId = getUserAccountId(myAccount);
            tx.Amount = amount;
            // check if transaction key is provided (for fee)
            if (key.HasValue)
                tx.Key = key.Value;
            else
                tx.Key = Guid.NewGuid();

            if (amount > 0)
            {
                tx.Type = TransactionType.Credit;
                if(otherAccount.HasValue)
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
                        addTransaction(myAccount, null, -_tax, tx.Key);
                }
            }

            _db.Transactions.Insert(tx);
            _transactions.Add(tx);
        }


        bool addUserAccount(string username, string userAccountName, Guid key)
        {
            if (userExist(username))
            {
                UserAccount account = new UserAccount();
                account.UserId = getUserID(username);
                account.Key = key;
                account.FriendlyName = userAccountName;
                account.Balance = 0;

                _userAccounts.Add(account);
                _db.UserAccounts.Insert(account);

                return true;
            }
            return false;
        }

        bool updateBalance(Guid account, decimal amount)
        {
            if (!userAccoutExist(account)) return false;

            UserAccount userAccount = getUserAccount(account);
            userAccount.Balance = Decimal.Add(userAccount.Balance, amount);
            _db.UserAccounts.Update(userAccount);

            return false;
        }


        int getUserID(string username)
        {
            return _users.Find(u => u.Username == username).Id;
        }

        bool userAccoutExist(Guid key)
        {
            return _userAccounts.Any(ua => ua.Key == key);
        }

        int getUserAccountId(Guid key)
        {
            return getUserAccount(key).Id;
        }

        decimal getUserBalance(Guid key)
        {
            return getUserAccount(key).Balance;
        }

        UserAccount getUserAccount(Guid key)
        {
            return _userAccounts.Find(ua => ua.Key == key);
        }

        bool userExist(string username)
        {
            return _users.Any(u => u.Username == username);
        }

        bool addUser(string username, string password, string fullname, string email)
        {
            if (validUsername(username))
            {
                User u = new User();
                u.Username = username;
                u.Password = password;
                u.FullName = fullname;
                u.Email = email;

                // Add new user
                _users.Add(u);
                _db.Users.Insert(u);
                return true;
            }
            return false;
        }

        bool validUsername(string username)
        {
            if (username.Length < 4 || username.Length > 16)
                return false;

            bool containsLetter = false;
            bool containsNumber = false;
            for(int i = 0; i < username.Length; i++)
            {
                if(username[i] >= 'a' && username[i] <= 'z' || username[i] >= 'A' && username[i] <= 'Z')
                {
                    if(! containsLetter)
                        containsLetter = true;
                }
                else if (username[i] >= '0' && username[i] <= '9')
                {
                    if (!containsNumber)
                        containsNumber = true;
                }
                else
                {
                    // contains invalid symbol
                    return false;
                }
            }
            
            // Doesn't contain any letter or number
            if (!containsLetter || !containsNumber)
                return false;

            // Already exists
            if (userExist(username))
                return false;

            return true;
        }

    }
}
