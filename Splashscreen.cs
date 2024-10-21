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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

      
        
        private void progressBar1_Click_1(object sender, EventArgs e)
        {
           

        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer1.Start();
            {
                if (progressBar1.Value < 100)
                {
                    progressBar1.Value++;
                    label2.Text = progressBar1.Value.ToString() + "%";

                    
                }
                else
                {
                    timer1.Stop();
                   inlog log = new inlog();
                    log.Show();
                    this.Hide();
                }
            }
        }
    }
}
