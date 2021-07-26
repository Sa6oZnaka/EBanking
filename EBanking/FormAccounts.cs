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

            displayUserAccounts();
        }

        private List<UserAccount> getUserAccounts()
        {
            return _users.UserAccounts.All.FindAll(ua => ua.UserId == _userID);
        }

        private void displayUserAccounts()
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
            _users.UserAccounts.add("Test name", _userID);
            displayUserAccounts();
        }

        private void FormAccounts_FormClosing(object sender, FormClosingEventArgs e)
        {
            buttonLogout.PerformClick();
        }
    }
}
