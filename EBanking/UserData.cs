using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using User = EBanking.Data.Entities.User;
using UserAccount = EBanking.Data.Entities.UserAccount;
using EBankingDbContext = EBanking.Data.EBankingDbContext;

namespace EBanking
{
    class UserData
    {

        List<User> _users;
        List<UserAccount> _userAccounts;
        EBankingDbContext _db = new EBankingDbContext("./db.txt");

        public UserData()
        {
            _users = _db.Users.All.ToList();
            _userAccounts = _db.UserAccounts.All.ToList();

            // Inserts
            //Console.WriteLine(addUser("Alex1", "12345", "Random Name", "test@test.test"));
            //Console.WriteLine(addUserAccount("Alex1", "Savings", Guid.NewGuid()));

            Console.WriteLine("Auth:  " + authenticate("Alex1", "123456"));
            Console.WriteLine("Auth:  " + authenticate("Alex1", "12345"));
            Console.WriteLine("Users: " + _users.Count);
            Console.WriteLine("UsersAccounts: " + _userAccounts.Count);
        }

        bool authenticate(string username, string password)
        {
            return _users.Any(u => u.Username == username && u.Password == password);
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

        int getUserID(string username)
        {
            return _users.Find(u => u.Username == username).Id;
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
