using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EBanking
{
    public partial class FormRegister : Form
    {

        private Users _users;

        public FormRegister(Users users)
        {
            InitializeComponent();

            _users = users;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            // register
            string username = this.textBoxUsername.Text;
            string password = this.textBoxPassword.Text;
            string confirmPassword = this.textBoxPassword.Text;
            string name = this.textBoxFullName.Text;
            string email = this.textBoxEmail.Text;

            if (validUsername(username) && validPassword(password, confirmPassword) && validEmail(email))
            {
                _users.add(username, password, name, email);
                this.Close();
            }
            
        }

        private bool validUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Username can't be empty!");
                return false;
            }
            if (username.Length < 4 || username.Length > 16)
            {
                MessageBox.Show("Username length must be between 4 and 16 characters!");
                return false;
            }

            bool containsLetter = false;
            bool containsNumber = false;
            for (int i = 0; i < username.Length; i++)
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
                {
                    MessageBox.Show("Username can only contain letters and digits!");
                    return false;
                }
            }

            if (!containsLetter || !containsNumber)
            {
                MessageBox.Show("Username must contain at least one letter and number!");
                return false;
            }
            if (_users.userExist(username))
            {
                MessageBox.Show("Username already used!");
                return false;
            }
            return true;
        }

        private bool validPassword(string password, string confirmPassword)
        {
            if (!password.Equals(confirmPassword))
            {
                MessageBox.Show("Password's don't match!");
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password can't be empty!");
                return false;
            }
            if (password.Length < 8)
            {
                MessageBox.Show("Password length must be more than 8 characters!");
                return false;
            }

            bool containsLetter = password.Any(p => char.IsLetter(p));
            bool containsNumber = password.Any(p => char.IsDigit(p));
            if (!containsLetter || !containsNumber)
            {
                MessageBox.Show("Password must contain at least one letter and number!");
                return false;
            }
            return true;
        }

        private bool validEmail(string email)
        {
            try
            {
                new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                MessageBox.Show("Invalid Email!");
                return false;
            }
        }
    }
}
