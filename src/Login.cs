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

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox2.Text;
            string password = textBox3.Text;
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Campos em branco", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        MessageBox.Show("Login bem-sucedido!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        Menu open = new Menu();
                        open.Show();
                    }
                    else
                    {
                        MessageBox.Show("Email ou password incorretos.", "Erro de login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {

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
             
            // Absolute folder you provided
            string folder = @"C:\Users\2224082\OneDrive - Escola Digital\Projeto Final C#\YGOShop_AfonsoEliseu_2224082\Imagens";
            string fileName = see ? "EyeClosed.png" : "Eye.png";
            string path = System.IO.Path.Combine(folder, fileName);

            if (!System.IO.File.Exists(path))
            {
                MessageBox.Show($"Image not found: {path}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load image into a new Bitmap (prevents file locking)
            var previous = pictureBox3.Image;
            using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (var tmp = Image.FromStream(fs))
                {
                    pictureBox3.Image = new Bitmap(tmp);
                }
            }
            previous?.Dispose();
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
        }
    }
}
