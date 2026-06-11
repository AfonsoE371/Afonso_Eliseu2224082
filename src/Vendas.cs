using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YGOShop_AfonsoEliseu_2224082
{
    public partial class Vendas : Form
    {
        int userId;
        public Vendas()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            panel1.AutoScroll = true;
            panel1.HorizontalScroll.Enabled = false;
            panel1.VerticalScroll.Enabled = true;

            MenuUsers menuUsers = new MenuUsers();
            userId = menuUsers.returnID();
            CarregarVendas(userId);
        }


        private void CarregarVendas(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(
                        @"SELECT v.Card_ID, c.Nome, v.Price, v.Copies
              FROM Vendas v
              JOIN Cards c ON v.Card_ID = c.Card_ID
              WHERE v.User_ID = @UserID", conn);

                    cmd.Parameters.AddWithValue("@UserID", userId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    panel1.Controls.Clear();

                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["Card_ID"]);
                        string nome = reader["Nome"].ToString();
                        double preco = Convert.ToDouble(reader["Price"]);
                        int copias = Convert.ToInt32(reader["Copies"]);

                        panel1.Controls.Add(CriarRegisto(id, nome, preco, copias));
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error loading sales: " + ex.Message, "Database error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has ocurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private Panel CriarRegisto(int id, string nome, double preco, int copias)
        {
            Panel item = new Panel();
            item.BackColor = Color.White;
            item.Width = panel1.Width - 25;
            item.Height = 60;
            item.Margin = new Padding(10);
            item.BorderStyle = BorderStyle.FixedSingle;

            Label lblId = new Label();
            lblId.Text = "CardId: " + id;
            lblId.Left = 20;
            lblId.Top = 20;

            Label lblNome = new Label();
            lblNome.Text = nome;
            lblNome.Left = 150;
            lblNome.Top = 20;

            Label lblPreco = new Label();
            lblPreco.Text = "Price: " + preco;
            lblPreco.Left = 350;
            lblPreco.Top = 20;

            Label lblCopias = new Label();
            lblCopias.Text = "Copies: " + copias;
            lblCopias.Left = 550;
            lblCopias.Top = 20;

            item.Controls.Add(lblId);
            item.Controls.Add(lblNome);
            item.Controls.Add(lblPreco);
            item.Controls.Add(lblCopias);

            return item;
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Criar_Vendas create = new Criar_Vendas();
            create.Show();
        }
    }
}
