
namespace EBanking
{
    partial class FormWithdraw
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonDeposit = new System.Windows.Forms.Button();
            this.textBoxWithdraw = new System.Windows.Forms.TextBox();
            this.labelText = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.textBoxWithdraw);
            this.panel2.Controls.Add(this.labelText);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(419, 320);
            this.panel2.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonDeposit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(5, 269);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(5);
            this.panel3.Size = new System.Drawing.Size(409, 46);
            this.panel3.TabIndex = 4;
            // 
            // buttonDeposit
            // 
            this.buttonDeposit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonDeposit.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonDeposit.Location = new System.Drawing.Point(319, 5);
            this.buttonDeposit.Name = "buttonDeposit";
            this.buttonDeposit.Size = new System.Drawing.Size(85, 36);
            this.buttonDeposit.TabIndex = 2;
            this.buttonDeposit.Text = "Withdraw";
            this.buttonDeposit.UseVisualStyleBackColor = true;
            this.buttonDeposit.Click += new System.EventHandler(this.buttonDeposit_Click);
            // 
            // textBoxWithdraw
            // 
            this.textBoxWithdraw.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxWithdraw.Location = new System.Drawing.Point(5, 18);
            this.textBoxWithdraw.Name = "textBoxWithdraw";
            this.textBoxWithdraw.Size = new System.Drawing.Size(409, 20);
            this.textBoxWithdraw.TabIndex = 0;
            // 
            // labelText
            // 
            this.labelText.AutoSize = true;
            this.labelText.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelText.Location = new System.Drawing.Point(5, 5);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(100, 13);
            this.labelText.TabIndex = 1;
            this.labelText.Text = "Amount to withdraw";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(6);
            this.panel1.Size = new System.Drawing.Size(431, 332);
            this.panel1.TabIndex = 5;
            // 
            // FormWithdraw
            // 
            this.AcceptButton = this.buttonDeposit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 332);
            this.Controls.Add(this.panel1);
            this.Name = "FormWithdraw";
            this.Text = "Withdraw";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWithdraw_FormClosing);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonDeposit;
        public System.Windows.Forms.TextBox textBoxWithdraw;
        public System.Windows.Forms.Label labelText;
        private System.Windows.Forms.Panel panel1;
    }
}