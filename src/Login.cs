using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YGOShop_AfonsoEliseu_2224082
{
    public partial class Login : Form
    {
        bool see = false;
        public Login()
        {

            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

            if (textBox3 != null)
                textBox3.UseSystemPasswordChar = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox2.Text;
            string password = textBox3.Text;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all the fields.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection("Server = (localdb)\\MSSQLLocalDB; Database = YGOShopDB; Trusted_Connection = True"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Email = @Email AND PasswordHash = @Password", conn);

                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    int result = (int)cmd.ExecuteScalar();
                    if (result > 0)
                    {

                        MenuUsers menuUsers = new MenuUsers();
                        menuUsers.register(email, password);
                        int num = menuUsers.GetUsername(email, password);
                        if (num == 1)
                        {
                            MessageBox.Show("Login Successful!!", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Menu menu = new Menu();
                            menu.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("User or ID not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (result == 0)
                    {
                        MessageBox.Show("Incorrect Email or Password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error has ocurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            see = !see;

            if (textBox3 != null)
                textBox3.UseSystemPasswordChar = !see;

           
            string basePath = Application.StartupPath;
            string imageFolder = Path.Combine(basePath, "Imagens");

            string fileName = see ? "Eye.png" : "EyeClosed.png";
            string path = Path.Combine(imageFolder, fileName);

            if (!File.Exists(path))
            {
                MessageBox.Show($"Image not found: {path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

           
            pictureBox3.Image?.Dispose();
            pictureBox3.Image = null;

            
            pictureBox3.Image = Image.FromFile(path);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;



        }
    }
}
