using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YGOShop_AfonsoEliseu_2224082
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {



        }

        private void label1_Click(object sender, EventArgs e)
        {



        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.Parent = pictureBox2;
            panel1.BackColor = Color.Transparent;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateA createA = new CreateA();
            createA.Show();
            this.Hide();
        }
    }
}
