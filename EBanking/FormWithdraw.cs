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
    public partial class FormWithdraw : Form
    {

        private UserAccounts _userAccounts;
        private decimal _amount;
        private Guid _address;
        private int _userID;

        public FormWithdraw(string address, int userID, UserAccounts userAccounts)
        {
            InitializeComponent();
            this.textBoxAddress.Text = address;

            _userAccounts = userAccounts;
            _userID = userID;
        }

        private void buttonDeposit_Click(object sender, EventArgs e)
        {
            if (valid())
            {
                _userAccounts.withdraw(_address, _amount);
                this.Close();
            }
        }

        private void FormWithdraw_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (!validate())
            //    e.Cancel = true;
        }

        private bool valid()
        {
            if (string.IsNullOrEmpty(this.textBoxWithdraw.Text))
            {
                MessageBox.Show("Amount can't be empty!");
                return false;
            }
            if (!decimal.TryParse(this.textBoxWithdraw.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Invalid amount!");
                return false;
            }
            _amount = amount;


            if (string.IsNullOrEmpty(this.textBoxAddress.Text))
            {
                MessageBox.Show("Address can't be empty!");
                return false;
            }
            if (!Guid.TryParse(this.textBoxAddress.Text, out Guid address))
            {
                MessageBox.Show("Invalid address!");
                return false;
            }
            if (!_userAccounts.userAccoutExist(address))
            {
                MessageBox.Show("User account doesn't exist!");
                return false;
            }
            if (!_userAccounts.All.Any(ua => ua.Key == address && ua.UserId == _userID))
            {
                MessageBox.Show("Can't access user account!");
                return false;
            }

            if ( decimal.Add(_userAccounts.getUserBalance(address), -_userAccounts.WithdrawFee) < amount)
            {
                MessageBox.Show("Insufficient funds!");
                return false;
            }

            _address = address;

            return true;
        }
    }
}
