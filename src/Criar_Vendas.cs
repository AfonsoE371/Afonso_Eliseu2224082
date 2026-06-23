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
    public partial class Criar_Vendas : Form
    {
        int? id = 0;
        string texto;
        int? level = 0;
        string tipo;
        string Attri;
        string CC;
        int? scales = 0;
        string PT;
        int? atk = 0;
        int? def = 0;
        bool isCardFound = false;
        string imageUrl = null;


        public Criar_Vendas()
        {
            InitializeComponent();
            textBox1.Text = null;

            MenuUsers menuUsers = new MenuUsers();
            textBox7.Text = menuUsers.returnUsername();
        }


        public void Criar_Vendas_Load()
        {
            textBox1.Text = null;
            MenuUsers menuUsers = new MenuUsers();
            textBox7.Text = menuUsers.returnUsername();
            textBox6.Text = null;
            textBox8.Text = null;
            richTextBox1.Text = null;
            richTextBox2.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox9.Text = null;
            textBox10.Text = null;
            textBox11.Text = null;
            richTextBox3.Text = null;
          
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
            Vendas vendas = new Vendas();
            vendas.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (id == 0)
            {
                MessageBox.Show("Card not present please choose one", "Card not selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (id == 0 && textBox1.Text == null)
            {
                MessageBox.Show("Please Confirm your selection for the card to be registered", "Card unconfirmed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }



            if (textBox6.Text == null)
            {
                MessageBox.Show("Please attribute a price to your sale.", "No price", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Would you like to use our delivery services (we will take 1€ from all your sales).", "Delivery Service", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {


            try
            {



                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();

                    string query = "SELECT Card_ID, Texto, Level, Attribute, MonsterType, CardCategory, Scales, PendulumText, Ataque, Defesa FROM Cards WHERE Nome = @Nome";
                    string query2 = "SELECT Image_URL FROM CardImages Where Card_ID = @Card_ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nome", textBox1.Text.Trim());

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                id = Convert.ToInt32(reader["Card_ID"]);

                                texto = reader["Texto"] != DBNull.Value ? reader["Texto"].ToString() : null;

                                level = reader["Level"] != DBNull.Value ? Convert.ToInt32(reader["Level"]) : (int?)null;

                                tipo = reader["MonsterType"] != DBNull.Value ? reader["MonsterType"].ToString() : null;

                                Attri = reader["Attribute"] != DBNull.Value ? reader["Attribute"].ToString() : null;

                                CC = reader["CardCategory"] != DBNull.Value ? reader["CardCategory"].ToString() : null;

                                scales = reader["Scales"] != DBNull.Value ? Convert.ToInt32(reader["Scales"]) : (int?)null;

                                PT = reader["PendulumText"] != DBNull.Value ? reader["PendulumText"].ToString() : null;

                                atk = reader["Ataque"] != DBNull.Value ? Convert.ToInt32(reader["Ataque"]) : (int?)null;

                                def = reader["Defesa"] != DBNull.Value ? Convert.ToInt32(reader["Defesa"]) : (int?)null;


                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(query2, conn))
                    {
                        cmd.Parameters.AddWithValue("@Card_ID", id);
                        using (SqlDataReader reader2 = cmd.ExecuteReader())
                        {
                            if (reader2.Read())
                            {
                                imageUrl = reader2["Image_URL"].ToString();
                                pictureBox2.Load(imageUrl);
                                if (imageUrl == null)
                                {
                                    MessageBox.Show("Card Image not found.", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    isCardFound = true;
                                }

                            }

                        }
                    }


                    if (id == 0 && isCardFound == false)
                    {
                        MessageBox.Show("Card not found, please confirm if the name is correct ", "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        richTextBox2.Text = texto;

                        textBox2.Text = CC;

                        textBox4.Text = tipo;

                        if (atk == -1)
                        {
                            textBox10.Text = "?";
                        }
                        else
                        {
                            textBox10.Text = atk.ToString();
                        }

                        if (def == -1)
                        {
                            textBox11.Text = "?";
                        }
                        else
                        {
                            textBox11.Text = def.ToString();
                        }

                        if (atk == null)
                        {
                            textBox10.Text = "null";
                        }
                        else
                        {
                            textBox10.Text = atk.ToString();
                        }

                        if (def == null)
                        {
                            textBox11.Text = "null";
                        }
                        else
                        {
                            textBox11.Text = def.ToString();
                        }

                        if (level == null)
                        {
                            textBox5.Text = "null";
                        }
                        else
                        {
                            textBox5.Text = level.ToString();
                        }

                        if (PT == null)
                        {
                            richTextBox3.Text = "null";
                        }
                        else
                        {
                            richTextBox3.Text = PT;
                        }

                        if (scales == null)
                        {
                            textBox9.Text = "null";
                        }
                        else
                        {
                            textBox9.Text = scales.ToString();
                        }

                        if (Attri == null)
                        {
                            textBox3.Text = "null";
                        }
                        else
                        {
                            textBox3.Text = Attri.ToString();
                        }

                        try
                        {

                        }
                        catch (SqlException exe)
                        {
                            MessageBox.Show("An error has ocurred in the database: " + exe.Message, "Sql Image related Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error has ocurred: " + ex.Message, "Image related Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }



                }
            }
            catch (SqlException exe)
            {
                MessageBox.Show("An error has ocurred in the database: " + exe.Message, "Sql Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has ocurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
