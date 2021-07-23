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
    class Users
    {

        IEBankingDbContext _db;
        UserAccounts _userAccounts;

        public List<User> All {
            get
            {
                return _db.Users.All.ToList();
            } 
        }

        public Users(IEBankingDbContext db)
        {
            _db = db;
            
            _userAccounts = new UserAccounts(db);
        }

        public UserAccounts UserAccounts {
            get
            {
                return _userAccounts;
            }
        }

        public void add(string username, string password, string fullname, string email)
        {
            validateUsername(username);
            
            User u = new User();
            u.Username = username;
            u.Password = password;
            u.FullName = fullname;
            u.Email = email;
            u.DateRegistered = DateTime.Now;

            _db.Users.Insert(u);
               
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
            return All.Any(u => u.Username == username && u.Password == password);
        }

        private int getUserID(string username)
        {
            return All.Find(u => u.Username == username).Id;
        }

        private bool userExist(string username)
        {
            return All.Any(u => u.Username == username);
        }

        private void validateUsername(string username)
        {
            if (username.Length < 4 || username.Length > 16)
                throw new InvalidOperationException("Username length must be between 4 and 16 characters!");

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
                    throw new InvalidOperationException("Username can only contain letters and digits!");
            }
            
            if (!containsLetter || !containsNumber)
                throw new InvalidOperationException("Username must contain at least one letter and number!");

            if (userExist(username))
                throw new InvalidOperationException("Username already used!");
        }

    }
}
