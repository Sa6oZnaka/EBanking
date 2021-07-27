using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EBanking.Data.Entities;
using EBanking.Data;
using EBanking.Data.Interfaces;

namespace EBanking
{
    public partial class FormAccounts : Form
    {

        private Users _users;
        private int _userID;

        public FormAccounts(int userID, Users users)
        {
            InitializeComponent();

            _userID = userID;
            _users = users;

            refreshUserAccounts();
        }

        private List<UserAccount> getUserAccounts()
        {
            return _users.UserAccounts.All.FindAll(ua => ua.UserId == _userID);
        }

        private void refreshUserAccounts()
        {
            List<UserAccount> accounts = getUserAccounts();
            //this.listBoxAccounts.Items.Clear();
            this.listViewAccounts.Items.Clear();

            foreach(UserAccount acc in accounts)
            {
                string[] row = { 
                    acc.FriendlyName, 
                    acc.Key.ToString(), 
                    acc.Balance.ToString() 
                };
                var listViewItem = new ListViewItem(row);
                listViewAccounts.Items.Add(listViewItem);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var fp = new FormTextBox();
            fp.labelText.Text = "Enter account name";
            if(fp.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(fp.textBox1.Text))
                {
                    MessageBox.Show("Name can't be empty!");
                    return;
                }
                _users.UserAccounts.add(fp.textBox1.Text, _userID);
            }

            refreshUserAccounts();
        }

        private void FormAccounts_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonLogout.PerformClick();
        }

        private void buttonDeposit_Click(object sender, EventArgs e)
        {
            if (listViewAccounts.SelectedItems.Count != 1)
            {
                MessageBox.Show("Select user account first!");
                return;
            }

            if(! Guid.TryParse(listViewAccounts.SelectedItems[0].SubItems[1].Text, out Guid key))
                throw new Exception("Can't parse user account key!");

            var fp = new FormTextBox();
            fp.labelText.Text = "Enter amount you want to deposit";
            if (fp.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(fp.textBox1.Text))
                {
                    MessageBox.Show("Amount can't be empty!");
                    return;
                }
                if (! decimal.TryParse(fp.textBox1.Text, out decimal result) && result <= 0)
                {
                    MessageBox.Show("Invalid amount!");
                    return;
                }
                _users.UserAccounts.deposit(key, result);
            }

            refreshUserAccounts();
        }
    }
}
