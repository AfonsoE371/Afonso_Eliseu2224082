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

        int saleId;
        bool right = false;

        public Vendas()
        {
            InitializeComponent();
            this.Load += Vendas_Load;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                new Menu().Show();  
            }

            base.OnFormClosing(e);   
        }

        private void Vendas_Load(object sender, EventArgs e)
        {


            MenuUsers menuUsers = new MenuUsers();
            userId = menuUsers.returnID();
            textBox1.Text = null;
            textBox2.Text = null;
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
                        @"SELECT v.Vendas_ID, v.Card_ID, c.Nome, v.Price, v.Copies
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
                        int vendaId = Convert.ToInt32(reader["Vendas_ID"]);
                        panel1.Controls.Add(CriarRegisto(vendaId, id, nome, preco, copias));
                    }
                    conn.Close();
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


        private Panel CriarRegisto(int vendaId, int id, string nome, double preco, int copias)
        {
            Panel item = new Panel();
            item.BackColor = Color.Gainsboro;
            item.Width = panel1.Width - 25;
            item.Height = 60;
            item.Margin = new Padding(8);
            item.BorderStyle = BorderStyle.FixedSingle;

            TableLayoutPanel table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;
            table.ColumnCount = 5;
            table.RowCount = 1;

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));


            Label lblVendaId = new Label()
            {
                Text = "Sale ID: " + vendaId,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblId = new Label()
            {
                Text = "CardId: " + id,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblNome = new Label()
            {
                Text = nome,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };

            Label lblPreco = new Label()
            {
                Text = "Price: " + preco,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblCopias = new Label()
            {
                Text = "Copies: " + copias,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };


            table.Controls.Add(lblVendaId, 0, 0);
            table.Controls.Add(lblId, 1, 0);
            table.Controls.Add(lblNome, 2, 0);
            table.Controls.Add(lblPreco, 3, 0);
            table.Controls.Add(lblCopias, 4, 0);

            item.Controls.Add(table);

            return item;
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Sale ID is required.", "Missing Field",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (!int.TryParse(textBox3.Text, out saleId))
                {
                    MessageBox.Show("Sale ID must be a valid number.", "Invalid Value",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (saleId < 0)
                {
                    MessageBox.Show("Sale ID cannot be negative.", "Below Zero",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    right = true;
                }
            }

            int? quantidade = null;
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (!int.TryParse(textBox1.Text, out int q))
                {
                    MessageBox.Show("Quantity must be a valid number.", "Invalid Value",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                   
                }
                if (q < 0)
                {
                    MessageBox.Show("Quantity cannot be negative.", "Below Zero",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                  
                }
                quantidade = q;
            }

            float? preco = null;
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (!float.TryParse(textBox2.Text, out float p))
                {
                    MessageBox.Show("Price must be a valid number.", "Invalid Value",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                   
                }
                if (p < 0)
                {
                    MessageBox.Show("Price cannot be negative.", "Below Zero",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    
                }
                preco = p;
            }

            if (right == true)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                    {
                        string campos = "";
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;

                        if (!string.IsNullOrWhiteSpace(textBox1.Text))
                        {
                            campos += "Copies = @Quantidade, ";
                            cmd.Parameters.AddWithValue("@Quantidade", int.Parse(textBox1.Text));
                        }
                        if (!string.IsNullOrWhiteSpace(textBox2.Text))
                        {
                            campos += "Price = @Preco, ";
                            cmd.Parameters.AddWithValue("@Preco", float.Parse(textBox2.Text));
                        }
                        if (campos == "")
                        {
                            MessageBox.Show("No fields were filled. Nothing to update.",
                                            "No Changes",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                           
                        }
                        campos = campos.TrimEnd(',', ' ');
                        cmd.Parameters.AddWithValue("@SaleId", int.Parse(textBox3.Text));
                        string query = "UPDATE Vendas SET " + campos + " WHERE Vendas_ID = @SaleId";
                        cmd.CommandText = query;

                        con.Open();
                        int linhas = cmd.ExecuteNonQuery();

                        if (linhas > 0)
                        {
                            MessageBox.Show("Sale updated successfully.",
                                            "Success",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                            
                            Vendas_Load(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("No sale found with that ID.",
                                            "Not Found",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Sale ID is required.", "Missing Field",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Vendas_Load(sender, e);
            }
            else
            {
                if (!int.TryParse(textBox3.Text, out saleId))
                {
                    MessageBox.Show("Sale ID must be a valid number.", "Invalid Value",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (saleId < 0)
                {
                    MessageBox.Show("Sale ID cannot be negative.", "Below Zero",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    right = true;
                }
            }

            if (right == true)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;

                        cmd.CommandText = "DELETE FROM Vendas WHERE Vendas_ID = @SaleId";
                        cmd.Parameters.AddWithValue("@SaleId", saleId);

                        con.Open();
                        int linhas = cmd.ExecuteNonQuery();

                        if (linhas > 0)
                        {
                            MessageBox.Show("Sale deleted successfully.",
                                            "Success",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);
                           
                            Vendas_Load(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("No sale found with that ID.",
                                            "Not Found",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
