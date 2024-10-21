using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Concertticketing
{
    public partial class inlog : Form
    {
        public inlog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            lbl1.Visible = false;
            lbl2.Visible = false;
            lbl3.Visible = false;
            if (string.IsNullOrEmpty(txtusername.Text))
            {
                lbl1.Visible = true;
            }
            if (string.IsNullOrEmpty(txtusername.Text))
            {
                lbl2.Visible = true;
            }
            if (txtusername.Text == "Admin" && txtpassword.Text == "admin12345")
            {
                MessageBox.Show("Login Success");
                records record = new records();
                record.Show();
                this.Hide();
            }
            else if (txtusername.Text == "Staff" && txtpassword.Text == "staff12345")
            {
                MessageBox.Show("Login Success");
                Cashier cashier = new Cashier();
                cashier.Show();
                this.Hide();
            }
            else
            {
                lbl3.Visible = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bool condition = checkBox1.Checked;
            if (condition)
            {
                txtpassword.PasswordChar = '\0';
            }
            else
            {
                txtpassword.PasswordChar = '•';

            }
        }

        private void txtusername_MouseClick(object sender, MouseEventArgs e)
        {
            lbluser.Visible = false;
        }

        private void txtpassword_MouseClick(object sender, MouseEventArgs e)
        {
            lblpass.Visible = false;
        }

        private void txtusername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtusername.Text))
            {
                lbluser.Visible = true;
            }
        }

        private void txtpassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtpassword.Text))
            {
                lblpass.Visible = true;
            }
        }

        private void txtusername_Enter(object sender, EventArgs e)
        {
            lbluser.Visible = false;
        }

        private void txtpassword_Enter(object sender, EventArgs e)
        {
            lblpass.Visible = false;
        }
    }
}
