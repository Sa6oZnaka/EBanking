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
    public partial class FormDeposit : Form
    {

        public decimal Amount {
            get
            {
                if (decimal.TryParse(this.textBoxDeposit.Text, out decimal result))
                    return result;
                else
                    throw new Exception("Can't parse textbox");
            }
        }

        public FormDeposit()
        {
            InitializeComponent();
        }

        private void buttonDeposit_Click(object sender, EventArgs e)
        {

        }

        private void FormDeposit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!validAmount())
                e.Cancel = true;
        }

        private bool validAmount()
        {
            if (string.IsNullOrEmpty(this.textBoxDeposit.Text))
            {
                MessageBox.Show("Amount can't be empty!");
                return false;
            }
            if (!decimal.TryParse(this.textBoxDeposit.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Invalid amount!");
                return false;
            }
            return true;
        }


    }
}
