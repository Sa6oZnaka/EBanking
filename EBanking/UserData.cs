using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using User = EBanking.Data.Entities.User;


namespace EBanking
{
    class UserData
    {

        List<User> _users;

        public UserData()
        {
            _users = new List<User>();
            Console.WriteLine(addUser("Alex1", "12345", "Random Name", "test@test.test"));
            Console.WriteLine(addUser("Alex1", "12345", "Random Name", "test@test.test"));
            Console.WriteLine(addUser("Alex2", "12345", "Random Name", "test@test.test"));

            Console.WriteLine("Users: " + _users.Count);
        }

        int getLastID()
        {
            return _users.Count;
        }

        bool addUser(string username, string password, string fullname, string email)
        {
            Console.WriteLine(validUsername(username));

            if (validUsername(username))
            {
                User u = new User();
                u.Id = getLastID();
                u.Username = username;
                u.Password = password;
                u.FullName = fullname;
                u.Email = email;
                // TODO get the date
                //u.DateRegistered = 
                _users.Add(u);
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
            if (_users.Any(u => u.Username == username))
                return false;

            return true;
        }




    }
}
