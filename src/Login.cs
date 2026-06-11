using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

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

            string folder = @"C:\Users\2224082\OneDrive - Escola Digital\Projeto Final C#\YGOShop_AfonsoEliseu_2224082\Imagens";
            string fileName = see ? "Eye.png" : "EyeClosed.png";
            string path = System.IO.Path.Combine(folder, fileName);

            if (!System.IO.File.Exists(path))
            {
                MessageBox.Show($"Image not found: {path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

           
            var previous = pictureBox3.Image;
            using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            using (var img = Image.FromStream(fs))
            {
                pictureBox3.Image = new Bitmap(img);
            }
            previous?.Dispose();
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;




        }
    }
}
