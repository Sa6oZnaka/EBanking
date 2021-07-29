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
    public partial class FormTransfer : Form
    {

        private UserAccounts _userAccounts;
        private Guid _receiverAddress;
        private Guid _senderAddress;
        private decimal _amount;
        private int _userID;

        public FormTransfer(string selected, int userID, UserAccounts userAccounts)
        {
            InitializeComponent();

            _userID = userID;
            _userAccounts = userAccounts;
            this.textBoxSender.Text = selected;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (valid())
            {
                _userAccounts.sendToUser(_senderAddress, _receiverAddress, _amount);
                this.Close();
            }

        }

        private void FormTransfer_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if(! valid())
            //    e.Cancel = true;
        }

        private bool valid()
        {
            // check receiver address
            if (! Guid.TryParse(this.textBoxAddress.Text, out Guid receiverKey))
            {
                MessageBox.Show("Can't parse user receiver address!");
                return false;
            }
            if (! _userAccounts.userAccoutExist(receiverKey))
            {
                MessageBox.Show("Receiver address doesn't exist!");
                return false;
            }
            _receiverAddress = receiverKey;

            // check if amount is valid
            if (string.IsNullOrEmpty(this.textBoxAmount.Text))
            {
                MessageBox.Show("Amount can't be empty!");
                return false;
            }
            if (!decimal.TryParse(this.textBoxAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Invalid amount!");
                return false;
            }

            _amount = amount;

            // check sender address
            if (string.IsNullOrEmpty(this.textBoxSender.Text ))
            {
                MessageBox.Show("Sender address can't be empty!");
                return false;
            }
            if (!Guid.TryParse(this.textBoxSender.Text, out Guid address))
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

            if (_userAccounts.getUserBalance(address) < amount)
            {
                MessageBox.Show("Insufficient funds!");
                return false;
            }

            _senderAddress = address;

            if (Guid.Equals(_senderAddress, _receiverAddress))
            {
                MessageBox.Show("Can't send to the same address!");
                return false;
            }

            return true;
        }

    }
}
