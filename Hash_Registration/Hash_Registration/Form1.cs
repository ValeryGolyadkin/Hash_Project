using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hash_Registration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        async private void Form1_Load(object sender, EventArgs e)
        {
            Form1 F = new Form1();

            //Form1 F = new Form1();
            for (float j = 0; j <= 10; j++)
            {
                for (float i = 0; i < 1; i += 0.05f, await Task.Delay(10))
                {
                    this.Opacity = i;
                }
                for (float i = 1; i >= 0; i -= 0.05f, await Task.Delay(10))
                {
                    this.Opacity = i;
                }
            }
            MessageBox.Show(Convert.ToString(F.Opacity));
        }
    }
}
