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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        private bool fecharForcado = false;
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!fecharForcado && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; 
            }
        }



        private void Menu_Load(object sender, EventArgs e)
        {
            try
            {
                MenuUsers menuUsers = new MenuUsers();
                int id = menuUsers.returnID();


                decimal saldoAtual = 0;

                string querySelect = "SELECT Saldo FROM Users WHERE User_ID = @id";

                using (SqlConnection conn = new SqlConnection("Server = (localdb)\\MSSQLLocalDB; Database = YGOShopDB; Trusted_Connection = True"))
                using (SqlCommand cmd = new SqlCommand(querySelect, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        saldoAtual = Convert.ToDecimal(result);
                }

                // mostrar na textbox
                textBox1.Text = saldoAtual.ToString("0.00");



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

                          
                            pictureBox1.Load(imageUrl);
                        }
                        else
                        {
                            MessageBox.Show("Profile Image not found please select one in the settings tab.");
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            fecharForcado = true;
            Comprar comprar = new Comprar();
            comprar.Show();
            this.Close();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            fecharForcado = true;
            Definições definições = new Definições();
            definições.Show();
            this.Close();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            fecharForcado = true;
            this.Close();
            Login log = new Login();
            log.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            fecharForcado = true;
            Vendas vendas = new Vendas();
            vendas.Show();
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            MenuUsers user = new MenuUsers();
            int userID = user.returnID();
            fecharForcado = true;
            Coleção coleção = new Coleção(userID);
            coleção.Show();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool right = false;
            decimal val = 0.00m;

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Value is Required.", "Missing Field",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (!decimal.TryParse(textBox2.Text, out val))
                {
                    MessageBox.Show("Value must be valid.", "Invalid Value",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (val < 0)
                {
                    MessageBox.Show("Value cannot be negative.", "Below Zero",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    right = true;
                }
            }

            try
            {
                if (right == true)
                {
                    MenuUsers user = new MenuUsers();
                    int id = user.returnID();



                    decimal saldoAtual = 0;

                    string querySelect = "SELECT Saldo FROM Users WHERE User_ID = @id";

                    using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                    using (SqlCommand cmd = new SqlCommand(querySelect, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                            saldoAtual = Convert.ToDecimal(result);
                    }

                    decimal novoSaldo = saldoAtual + val;

                    string queryUpdate = "UPDATE Users SET Saldo = @novoSaldo WHERE User_ID = @id";

                    using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                    using (SqlCommand cmd = new SqlCommand(queryUpdate, conn))
                    {
                        cmd.Parameters.AddWithValue("@novoSaldo", novoSaldo);
                        cmd.Parameters.AddWithValue("@id", id);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Balance Updated Successfully.", "Success",
                                                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Menu_Load(sender, e);
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

        private void button1_Click(object sender, EventArgs e)
        {
            bool right = false;
            decimal valorInserido = 0.00m;



            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Value is Required.", "Missing Field",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (!decimal.TryParse(textBox2.Text, out valorInserido))
                {
                    MessageBox.Show("Value must be valid.", "Invalid Value",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (valorInserido < 0)
                {
                    MessageBox.Show("Value cannot be negative.", "Below Zero",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    right = true;
                }
            }

            try
            {
                if (right)
                {


                    MenuUsers user = new MenuUsers();
                    int id = user.returnID();

                    decimal saldoAtual = 0;

                    string querySelect = "SELECT Saldo FROM Users WHERE User_ID = @id";

                    using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                    using (SqlCommand cmd = new SqlCommand(querySelect, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                            saldoAtual = Convert.ToDecimal(result);
                    }


                    decimal novoSaldo = saldoAtual - valorInserido;


                    if (novoSaldo < 0)
                    {
                        MessageBox.Show("Balance cannot go Below Zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }



                    string queryUpdate = "UPDATE Users SET Saldo = @novoSaldo WHERE User_ID = @id";

                    using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                    using (SqlCommand cmd = new SqlCommand(queryUpdate, conn))
                    {
                        cmd.Parameters.AddWithValue("@novoSaldo", novoSaldo);
                        cmd.Parameters.AddWithValue("@id", id);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Balance Updated Successfully.", "Success",
                                                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Menu_Load(sender, e);
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
