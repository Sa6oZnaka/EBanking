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

        private Users _users;
        private Guid _address;

        public Guid Address {
            get
            {
                if (Guid.TryParse(this.textBoxAddress.Text, out Guid result))
                    return result;
                else
                    throw new Exception("Can't parse user address!");
            }
        }

        public decimal Amount
        {
            get
            {
                if (decimal.TryParse(this.textBoxAmount.Text, out decimal result))
                    return result;
                else
                    throw new Exception("Can't parse textbox");
            }
        }

        public FormTransfer(Users users, Guid address)
        {
            InitializeComponent();

            _users = users;
            _address = address;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {

        }

        private void FormTransfer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(! valid())
                e.Cancel = true;
        }

        private bool valid()
        {
            // check address
            if (! Guid.TryParse(this.textBoxAddress.Text, out Guid receiverKey))
            {
                MessageBox.Show("Can't parse user receiver address!");
                return false;
            }
            if (!_users.UserAccounts.userAccoutExist(receiverKey))
            {
                MessageBox.Show("Receiver address doesn't exist!");
                return false;
            }

            // check if amount is valid
            if (string.IsNullOrEmpty(this.textBoxAmount.Text))
            {
                MessageBox.Show("Amount can't be empty!");
                return false;
            }
            if (!decimal.TryParse(this.textBoxAmount.Text, out decimal amount) && amount <= 0)
            {
                MessageBox.Show("Invalid amount!");
                return false;
            }
            if (Decimal.Add(_users.UserAccounts.getUserBalance(_address), -amount) < 0)
            {
                MessageBox.Show("Insufficient funds!");
                return false;
            }

            return true;
        }

    }
}
