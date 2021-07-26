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
    public partial class FormLogin : Form
    {

        Users _users;
        IEBankingDbContext _db;

        public FormLogin()
        {
            InitializeComponent();

            _db = new EBankingDbContext("./db.txt");
            _users = new Users(_db);
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            if(_users.authenticate(username, password))
            {
                var accountsForm = new FormAccounts(_users);
                accountsForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong username or password!");
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            var registerForm = new FormRegister(_users);
            registerForm.Show();
        }
    }
}
