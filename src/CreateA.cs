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
using static System.Net.Mime.MediaTypeNames;

namespace YGOShop_AfonsoEliseu_2224082
{
    public partial class CreateA : Form
    {
        public bool sair = false;

        public CreateA()
        {

            InitializeComponent();
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;



        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                MessageBox.Show("Field not filled: Name", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox2.Text == null)
            {
                MessageBox.Show("Field not filled: Email", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox3.Text == null)
            {
                MessageBox.Show("Field not filled: Password", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!textBox2.Text.Contains("@"))
            {
                MessageBox.Show("@ needed for email validation.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox3.Text.Length < 8)
            {
                MessageBox.Show("Password must contain atleast 8 characters.", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                sair = true;
            }

            if (sair == true)
            {
                
                try
                {
                    SqlConnection conn = new SqlConnection("Server = (localdb)\\MSSQLLocalDB; Database = YGOShopDB; Trusted_Connection = True");
                    {
                        conn.Open();
                        SqlCommand insert = new SqlCommand(
                            "INSERT INTO Users (Username, Email, PasswordHash) VALUES (@username, @email, @password)", conn);

                        insert.Parameters.Add("@username", System.Data.SqlDbType.NVarChar).Value = textBox1.Text;
                        insert.Parameters.Add("@email", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                        insert.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = textBox3.Text;

                        insert.ExecuteNonQuery();
                    }
                    MessageBox.Show("Account Created Successfully!!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sair = false;
                this.Close();
                Login login = new Login();
                login.Show();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                    {
                        MessageBox.Show("Username or email already exists.");
                    }
                    else
                    {
                        MessageBox.Show($"An Error ocurred try again later", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
           
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();

        }
    }
}
