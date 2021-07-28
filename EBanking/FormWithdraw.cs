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

        private decimal _balance;

        public decimal Amount {
            get
            {
                if (decimal.TryParse(this.textBoxWithdraw.Text, out decimal result))
                    return result;
                else
                    throw new Exception("Can't parse textbox");
            }
        }

        public FormWithdraw(decimal balance)
        {
            InitializeComponent();

            _balance = balance;
        }

        private void buttonDeposit_Click(object sender, EventArgs e)
        {

        }

        private void FormWithdraw_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!validAmount())
                e.Cancel = true;
        }

        private bool validAmount()
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
            if (_balance < amount)
            {
                MessageBox.Show("Insufficient funds!");
                return false;
            }
            return true;
        }

    }
}
