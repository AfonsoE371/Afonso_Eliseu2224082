using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YGOShop_AfonsoEliseu_2224082
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();

            Utilidades utilidades = new Utilidades();
            utilidades.arredondar(pictureBox1);

        }

        private void Menu_Load(object sender, EventArgs e)
        {

            MenuUsers menuUsers = new MenuUsers();

            string query = @"
            SELECT ci.Image_Cropped_URL
            FROM UserProfileImage upi
            JOIN CardImages ci ON upi.Image_ID = ci.Image_ID
            WHERE upi.User_ID = @UserID";

            using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", menuUsers.returnID());

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string imageUrl = result.ToString();

                        // Carregar imagem no PictureBox
                        pictureBox1.Load(imageUrl);
                    }
                    else
                    {
                        MessageBox.Show("Profile Image not found please select one in the settings tab.");
                    }
                }
            }



            //https://images.ygoprodeck.com/images/cards/81344637.jpg
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Comprar comprar = new Comprar();
            comprar.Show();
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Definições definições = new Definições();
            definições.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Vendas vendas = new Vendas();
            vendas.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Coleção coleção = new Coleção();
            coleção.Show();
            this.Hide();
        }
    }
}
