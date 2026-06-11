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

        public Criar_Vendas()
        {
            InitializeComponent();
        }


        public void Criar_Vendas_Load()
        {
            MenuUsers menuUsers = new MenuUsers();
            textBox7.Text = menuUsers.returnUsername();
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
                }

                if (atk == -1)
                {
                    textBox10.Text = "?";  
                }

                if (def == -1)
                {
                    textBox11.Text = "?";
                }

                if (atk == null)
                {
                    textBox10.Text = "null";
                }

                if (def == null)
                {
                    textBox11.Text = "null";
                }

                if (level == null)
                {
                    textBox5.Text = "null";
                }

                if (PT == null)
                {
                    richTextBox3.Text = "null";
                }

                if (scales == null)
                {
                    textBox9.Text = "null";
                }

                if (Attri == null)
                {
                    textBox3.Text = "null";
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
