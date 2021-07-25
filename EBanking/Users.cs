using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EBanking.Data.Entities;
using EBanking.Data;
using EBanking.Data.Interfaces;

using System.Security.Cryptography;

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
            using (var transaction = _db.StartDbTransaction())
            {
                validateUsername(username);
                validateEmail(email);
                if (password.Length < 8)
                    throw new InvalidOperationException("Password length must be more than 8 characters!");

                User u = new User();
                u.Username = username;
                u.FullName = fullname;
                u.Email = email;
                u.DateRegistered = DateTime.Now;

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    string hash = GetHash(sha256Hash, password);
                    u.Password = hash;
                }

                _db.Users.Insert(u);
                transaction.Commit();
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            var hashOfInput = GetHash(hashAlgorithm, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }

        public void addUserAccount(string username, string userAccountName)
        {
            if (! userExist(username))
                throw new Exception("User doesn't exist!");

            _userAccounts.add(userAccountName, getUserID(username));
        }

        public bool authenticate(string username, string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return All.Any(u => u.Username == username && VerifyHash(sha256Hash, password, u.Password));
            }
        }

        private int getUserID(string username)
        {
            return All.Find(u => u.Username == username).Id;
        }

        private bool userExist(string username)
        {
            return All.Any(u => u.Username == username);
        }

        private void validateEmail(string email)
        {
            try
            {
                new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                throw new Exception("Invalid Email!");
            }
        }

        private void validateUsername(string username)
        {
            if(string.IsNullOrEmpty(username))
                throw new Exception("Username can't be empty!");

            if (username.Length < 4 || username.Length > 16)
                throw new Exception("Username length must be between 4 and 16 characters!");

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
                    throw new Exception("Username can only contain letters and digits!");
            }
            
            if (!containsLetter || !containsNumber)
                throw new Exception("Username must contain at least one letter and number!");

            if (userExist(username))
                throw new Exception("Username already used!");
        }

    }
}
