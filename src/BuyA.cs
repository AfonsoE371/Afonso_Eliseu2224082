using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.VisualBasic;

namespace YGOShop_AfonsoEliseu_2224082
{
    public partial class BuyA : Form
    {

        int vendaID;
        int cardID;
        int sellerID;
        int currentUserID;
        double price;
        int copiesAvailable;
        int deliv;

        public BuyA(int saleID)
        {
            InitializeComponent();
            richTextBox1.WordWrap = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBox2.WordWrap = true;
            richTextBox2.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBox3.WordWrap = true;
            richTextBox3.ScrollBars = RichTextBoxScrollBars.Vertical;




            vendaID = saleID;






            currentUserID = new MenuUsers().returnID();

            LoadSaleData();
            LoadCardData();
            LoadSellerData();

        }
        public void BuyA_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();

                    string query = "SELECT Saldo FROM Users WHERE User_ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", currentUserID);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            double saldo = Convert.ToDouble(result);
                            textBox12.Text = saldo.ToString("0.00") + "€";
                        }
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




        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Comprar compre = new Comprar();
            compre.Show();
        }

        private void LoadSaleData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();

                    string query = @"SELECT User_ID, Card_ID, Price, Copies,Deliv
                             FROM Vendas WHERE Vendas_ID = @Vendas_ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Vendas_ID", vendaID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                deliv = Convert.ToInt32(reader["Deliv"]);
                                sellerID = Convert.ToInt32(reader["User_ID"]);
                                cardID = Convert.ToInt32(reader["Card_ID"]);
                                price = Convert.ToDouble(reader["Price"]);
                                copiesAvailable = Convert.ToInt32(reader["Copies"]);

                                textBox8.Text = price.ToString("0.00") + "€";
                                textBox6.Text = copiesAvailable.ToString();
                            }
                        }
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

        private void LoadCardData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();

                    string query = @"SELECT Nome, Texto, Level, Attribute, MonsterType, CardCategory,
                             Scales, PendulumText, Ataque, Defesa
                             FROM Cards WHERE Card_ID = @Card_ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Card_ID", cardID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox1.Text = reader["Nome"].ToString();
                                richTextBox2.Text = reader["Texto"].ToString();
                                textBox5.Text = reader["Level"].ToString();
                                textBox3.Text = reader["Attribute"].ToString();
                                textBox4.Text = reader["MonsterType"].ToString();
                                textBox2.Text = reader["CardCategory"].ToString();
                                textBox9.Text = reader["Scales"].ToString();
                                richTextBox3.Text = reader["PendulumText"].ToString();
                                textBox10.Text = reader["Ataque"].ToString();
                                textBox11.Text = reader["Defesa"].ToString();
                            }
                        }
                    }

                    string queryImg = "SELECT Image_URL FROM CardImages WHERE Card_ID = @Card_ID";

                    using (SqlCommand cmd = new SqlCommand(queryImg, conn))
                    {
                        cmd.Parameters.AddWithValue("@Card_ID", cardID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string url = reader["Image_URL"].ToString();
                                using (WebClient wc = new WebClient())
                                {
                                    byte[] data = wc.DownloadData(url);
                                    using (MemoryStream ms = new MemoryStream(data))
                                    {
                                        pictureBox2.Image = Image.FromStream(ms);
                                    }
                                }
                            }
                        }
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

        private void LoadSellerData()
        {

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();

                    string query = "SELECT Username, Saldo FROM Users WHERE User_ID = @User_ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@User_ID", sellerID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textBox7.Text = reader["Username"].ToString();

                            }
                        }
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



        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (sellerID == currentUserID)
            {
                MessageBox.Show("You cannot buy your own card.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();

                    double userBalance = 0;

                    using (SqlCommand cmd = new SqlCommand("SELECT Saldo FROM Users WHERE User_ID = @ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", currentUserID);
                        userBalance = Convert.ToDouble(cmd.ExecuteScalar());
                    }

                    if (userBalance < price)
                    {
                        MessageBox.Show("You do not have enough balance.", "Insufficient Funds", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (copiesAvailable <= 0)
                    {
                        MessageBox.Show("No copies available.", "Sold Out", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }


                    bool entregaAtiva = deliv == 1;


                    double valorRecebido = entregaAtiva ? price - 1 : price;


                    string morada = Microsoft.VisualBasic.Interaction.InputBox(
                        "Insert your address for delivery:",
                        "Delivery Address",
                        ""
                    );

                    if (string.IsNullOrWhiteSpace(morada))
                    {
                        MessageBox.Show("The delivery address is required to complete the purchase.");
                        return;
                    }

                    using (SqlCommand cmd = new SqlCommand("UPDATE Users SET Saldo = Saldo - @Price WHERE User_ID = @ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@ID", currentUserID);
                        cmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("UPDATE Users SET Saldo = Saldo + @ValorRecebido WHERE User_ID = @ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ValorRecebido", valorRecebido);
                        cmd.Parameters.AddWithValue("@ID", sellerID);
                        cmd.ExecuteNonQuery();
                    }


                    using (SqlCommand cmd = new SqlCommand("UPDATE Vendas SET Copies = Copies - 1 WHERE Vendas_ID = @Vendas_ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@Vendas_ID", vendaID);
                        cmd.ExecuteNonQuery();
                    }

                    MenuUsers menu = new MenuUsers();


                    string compradorNome = menu.GetUsernameBuy(currentUserID);
                    string vendedorNome = menu.GetUsernameBuy(sellerID);

               
                    if (string.IsNullOrWhiteSpace(compradorNome))
                    {
                        MessageBox.Show("Erro: o username do comprador não foi encontrado.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(vendedorNome))
                    {
                        MessageBox.Show("Erro: o username do vendedor não foi encontrado.");
                        return;
                    }

                 
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        compradorNome = compradorNome.Replace(c, '_');
                        vendedorNome = vendedorNome.Replace(c, '_');
                    }

                   
                    string pastaComprador = Path.Combine(Application.StartupPath, compradorNome);
                    string pastaVendedor = Path.Combine(Application.StartupPath, vendedorNome);

             
                    Directory.CreateDirectory(pastaComprador);
                    Directory.CreateDirectory(pastaVendedor);
                    string talaoComprador =
                        $"--- RECEIPT FOR BUYER ---\n" +
                        $"Delivery Address: {morada}\n" +
                        $"Price Paid: {price}€\n" +
                        $"Delivery Service: {(entregaAtiva ? "Active (fee applied)" : "Standard")}\n" +
                        $"Date: {DateTime.Now}\n";

                    string talaoVendedor =
                        $"--- RECEIPT FOR SELLER ---\n" +
                        $"Delivery Address: {morada}\n" +
                        $"Amount Received: {valorRecebido}€\n" +
                        $"Delivery Service: {(entregaAtiva ? "Active (fee applied)" : "Standard")}\n" +
                        $"Date: {DateTime.Now}\n";

                    string ficheiroComprador = Path.Combine(pastaComprador, "talao_comprador.txt");
                    string ficheiroVendedor = Path.Combine(pastaVendedor, "talao_vendedor.txt");

                    File.WriteAllText(ficheiroComprador, talaoComprador);
                    File.WriteAllText(ficheiroVendedor, talaoVendedor);


                    MessageBox.Show("Purchase successful!\nReceipts sent to both parties.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
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
            LoadSaleData();
            LoadCardData();
            LoadSellerData();
            BuyA_Load(this, EventArgs.Empty);
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
