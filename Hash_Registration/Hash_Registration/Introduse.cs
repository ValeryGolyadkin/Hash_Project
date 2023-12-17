using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Hash_Registration
{
    public partial class Introduse : Form
    {
        bool ShowPassword = false;
        bool Registration = false;

        public class DB_USERS
        {
            public int _ID { get; set; }
            public string _LOGIN { get; set; }
            public string _PASSWORD { get; set; }
            public char _ACCESS { get; set; }

        }
        public Introduse()
        {
            InitializeComponent();
        }

        private void Introduse_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (ShowPassword == true)
            {
                textBox2.PasswordChar = '*';
                ShowPassword = false;
            }
            else { ShowPassword = true; textBox2.PasswordChar = '\0'; }
            textBox1.Text = Convert.ToString(ShowPassword);
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            if (ShowPassword == true)
            {
                label5.ForeColor = Color.Blue;

            }
            else
            {
                label5.ForeColor = Color.Gray;
            }
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            if (ShowPassword == true)
            {
                label5.ForeColor = Color.Red;

            }
            else
            {
                label5.ForeColor = Color.Black;
            }
        }
        private void label5_MouseDown(object sender, MouseEventArgs e)
        {
            label5.ForeColor = Color.Black;
        }

        private void label5_MouseUp(object sender, MouseEventArgs e)
        {
            if (ShowPassword == true)
            {
                label5.ForeColor = Color.Blue;
            }
            else
            {
                label5.ForeColor = Color.Black;
            }
        }

        async private void label4_Click(object sender, EventArgs e)
        {
            label4.Visible = false;
            int R = 0;
            int G = 0;
            int B = 0;
            int R_panel1 = 0;
            int G_panel1 = 0;
            int B_panel1 = 0;

            R = this.BackColor.R;
            G = this.BackColor.G;
            B = this.BackColor.B;
            R_panel1 = panel1.BackColor.R;
            G_panel1 = panel1.BackColor.G;
            B_panel1 = panel1.BackColor.B;
            if (Registration == false) {
                for (int i = 0; i < 25; i++, B+=2, B_panel1 += 2, await Task.Delay(1))
                {
                    if (R > 255) { R = 0; } else if (R < 0) { R = 255; }
                    if (G > 255) { G = 0; } else if (G < 0) { G = 255; }
                    if (B > 255) { B = 0; } else if (B < 0) { B = 255; }
                    panel1.BackColor = Color.FromArgb(R_panel1, G_panel1, B_panel1);
                    this.BackColor = Color.FromArgb(R, G, B);
                }
                Registration = true;
            }
            button3.Visible = true;
        }
        async private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            int R = 0;
            int G = 0;
            int B = 0;
            int R_panel1 = 0;
            int G_panel1 = 0;
            int B_panel1 = 0;

            R = this.BackColor.R;
            G = this.BackColor.G;
            B = this.BackColor.B;
            R_panel1 = panel1.BackColor.R;
            G_panel1 = panel1.BackColor.G;
            B_panel1 = panel1.BackColor.B;

            if (Registration == true)
            {
                for (int i = 0; i < 25; i++, B-=2, B_panel1-=2, await Task.Delay(1))
                {
                    if (R > 255) { R = 0; } else if (R < 0) { R = 255; }
                    if (G > 255) { G = 0; } else if (G < 0) { G = 255; }
                    if (B > 255) { B = 0; } else if (B < 0) { B = 255; }
                    panel1.BackColor = Color.FromArgb(R_panel1, G_panel1, B_panel1);
                    this.BackColor = Color.FromArgb(R, G, B);
                }
                Registration = false;
            }
            label4.Visible = true;
            //MessageBox.Show(Convert.ToString(B));
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {

        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<DB_USERS> Db_write = new List<DB_USERS>();

            string fileName_r = "DB_USERS.json";
            string jsonString_r = File.ReadAllText(fileName_r);
            DB_USERS Db_read = JsonSerializer.Deserialize<DB_USERS>(jsonString_r);
            MessageBox.Show(Convert.ToString(Db_read._ACCESS));
            if (textBox1.Text != "" & textBox2.Text != "") {
                //this.Location = MousePosition;
                Db_write.Add(new DB_USERS
                {
                    _ID = 1,
                    _LOGIN = textBox1.Text,
                    _PASSWORD = textBox2.Text,
                    _ACCESS = 'A'
                }) ;

                string fileName = "DB_USERS.json";
                string jsonString = JsonSerializer.Serialize(Db_write);
                File.AppendAllText(fileName, "List:" + jsonString + "\n");
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private void Introduse_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
