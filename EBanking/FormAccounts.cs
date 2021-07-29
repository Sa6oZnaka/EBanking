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
        private void refreshUserAccounts()
        {
            List<UserAccount> accounts = _users.UserAccounts.All.FindAll(ua => ua.UserId == _userID);

            // add new account if user doesn't have any accounts
            if (accounts.Count == 0)
            {
                var fp = new FormAccount();
                if (fp.ShowDialog() == DialogResult.OK)
                {
                    _users.UserAccounts.add(fp.AccountName, _userID);
                    accounts = _users.UserAccounts.All.FindAll(ua => ua.UserId == _userID);
                }
            }
            
            this.listViewAccounts.Items.Clear();

            foreach (UserAccount acc in accounts)
            {
                string[] row = { 
                    acc.FriendlyName, 
                    acc.Key.ToString(), 
                    acc.Balance.ToString() 
                };
                var listViewItem = new ListViewItem(row);
                listViewAccounts.Items.Add(listViewItem);
            }

            refreshTransactions();
        }

        private void refreshTransactions()
        {
            List<UserAccount> accounts = _users.UserAccounts.All.FindAll(ua => ua.UserId == _userID);
            List<Transaction> transactions = _users.Transactions.All;

            var userTransactions = accounts.Join(transactions,
                acc => acc.Id,
                tx => tx.UserAccountId,
                (acc, tx) =>
                    tx);

            userTransactions = userTransactions.OrderBy(tx => tx.EventDate).Reverse();
            this.listViewTransactions.Items.Clear();

            foreach (Transaction tx in userTransactions)
            {
                string[] row = {
                    tx.EventDate.ToString(),
                    tx.Key.ToString(),
                    tx.SystemComment,
                    tx.Type.ToString(),
                    tx.Amount.ToString()
                };
                var listViewItem = new ListViewItem(row);
                listViewTransactions.Items.Add(listViewItem);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var fp = new FormAccount();
            if(fp.ShowDialog() == DialogResult.OK)
            {
                _users.UserAccounts.add(fp.AccountName, _userID);
            }

            refreshUserAccounts();
        }

        private void FormAccounts_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonLogout.PerformClick();
        }

        private void buttonDeposit_Click(object sender, EventArgs e)
        {
            string address = "";
            if (listViewAccounts.SelectedItems.Count == 1)
                address = listViewAccounts.SelectedItems[0].SubItems[1].Text;

            var fp = new FormDeposit(address, _userID, _users);
            fp.Show();
            fp.FormClosing += new FormClosingEventHandler(RefreshUserAccounts);
        }

        private void RefreshUserAccounts(object sender, FormClosingEventArgs e)
        {
            refreshUserAccounts();
        }

        private void buttonWithdraw_Click(object sender, EventArgs e)
        {
            string address = "";
            if (listViewAccounts.SelectedItems.Count == 1)
                address = listViewAccounts.SelectedItems[0].SubItems[1].Text;

            var fp = new FormWithdraw(address, _userID, _users);
            fp.Show();
            fp.FormClosing += new FormClosingEventHandler(RefreshUserAccounts);
        }

        private void buttonTransfer_Click(object sender, EventArgs e)
        {
            string address = "";
            if (listViewAccounts.SelectedItems.Count == 1)
                address = listViewAccounts.SelectedItems[0].SubItems[1].Text;

            var fp = new FormTransfer(address, _userID, _users);
            fp.Show();
            fp.FormClosing += new FormClosingEventHandler(RefreshUserAccounts);
        }
    }
}
