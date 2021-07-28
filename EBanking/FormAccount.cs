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
    public partial class FormAccount : Form
    {

        public string AccountName {
            get
            {
                return this.textBoxAccount.Text;
            }
        }

        public FormAccount()
        {
            InitializeComponent();
        }

        private void FormAccount_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (! validName())
                e.Cancel = true;
        }

        private bool validName()
        {
            if (string.IsNullOrEmpty(AccountName)) {
                MessageBox.Show("Account name can't be empty!");
                return false;
            }
            return true;
        }
    }
}
