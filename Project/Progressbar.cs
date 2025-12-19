using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Progressbar : Form
    {
        public Progressbar()
        {
            InitializeComponent();
        }

       
        

        private void Progressbar_Load(object sender, EventArgs e)
        {
            guna2ProgressBar1.Value = 0;      
            guna2ProgressBar1.Maximum = 100;  

            timer1.Interval = 50;
            timer1.Start();               
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            guna2ProgressBar1.Increment(1);

            if (guna2ProgressBar1.Value < 100)
            {
                guna2ProgressBar1.Value += 1;
                label3.Text = guna2ProgressBar1.Value.ToString() + "%";

            }
            else
            {
                timer1.Stop();
                Form1 logfrm = new Form1();
                logfrm.Show();
                this.Hide();
            }

        }
    }


}

