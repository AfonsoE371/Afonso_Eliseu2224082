using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace YGOShop_AfonsoEliseu_2224082
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            pictureBox1.Load("https://images.ygoprodeck.com/images/cards_cropped/46396218.jpg");
            Utilidades utilidades = new Utilidades();
            utilidades.arredondar(pictureBox1);
        }

        private void Menu_Load(object sender, EventArgs e)
        {

            //https://images.ygoprodeck.com/images/cards/81344637.jpg
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
