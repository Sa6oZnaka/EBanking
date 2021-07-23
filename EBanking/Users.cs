using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using User = EBanking.Data.Entities.User;

using EBankingDbContext = EBanking.Data.EBankingDbContext;

namespace EBanking
{
    class Users
    {
        List<User> _users;
        EBankingDbContext _db;
        UserAccounts _userAccounts;

        public UserAccounts UserAccounts {
            get
            {
                return _userAccounts;
            }
        }

        public List<User> All {
            get
            {
                return _users;
            }
        }

        public Users(EBankingDbContext db)
        {
            _db = db;
            _users = _db.Users.All.ToList();

            _userAccounts = new UserAccounts(db);
        }
        public bool add(string username, string password, string fullname, string email)
        {
            if (validUsername(username))
            {
                User u = new User();
                u.Id = _users.Count;
                u.Username = username;
                u.Password = password;
                u.FullName = fullname;
                u.Email = email;
                u.DateRegistered = DateTime.Now;

                // Add new user
                _users.Add(u);
                _db.Users.Insert(u);
                return true;
            }
            return false;
        }

        public bool addUserAccount(string username, string userAccountName, Guid key)
        {
            if (userExist(username))
            {
                _userAccounts.add(username, userAccountName, key, getUserID(username));
                return true;
            }
            return false;
        }


        public bool authenticate(string username, string password)
        {
            return _users.Any(u => u.Username == username && u.Password == password);
        }

        private int getUserID(string username)
        {
            return _users.Find(u => u.Username == username).Id;
        }

        private bool userExist(string username)
        {
            return _users.Any(u => u.Username == username);
        }

        private bool validUsername(string username)
        {
            if (username.Length < 4 || username.Length > 16)
                return false;

            bool containsLetter = false;
            bool containsNumber = false;
            for(int i = 0; i < username.Length; i++)
            {
                if (Char.IsLetter(username[i]))
                {
                    if (!containsLetter)
                        containsLetter = true;
                }
                else if (Char.IsDigit(username[i]))
                {
                    if (!containsNumber)
                        containsNumber = true;
                }
                else
                    return false;
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
