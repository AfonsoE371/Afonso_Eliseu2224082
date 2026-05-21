using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace YGOShop_AfonsoEliseu_2224082
{
    public partial class Definições : Form
    {
        public string url;

        public Definições()
        {

            InitializeComponent();
            Utilidades utilidades = new Utilidades();
            utilidades.arredondar(pictureBox1);

            // attach the actual Paint handler (not a null field)
            pictureBox1.Paint += pictureBox1_Paint_1;

            // Ensure the picture box starts blank and scaled
            pictureBox1.ErrorImage = null;
            pictureBox1.Image = null;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;  

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();

                    string query = @"SELECT C.Nome, CI.Image_Cropped_URL FROM Cards C INNER JOIN CardImages CI  ON C.Card_ID = CI.Card_ID";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "Nome";
                    comboBox1.ValueMember = "Image_Cropped_URL";

                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao conectar ao banco de dados: " + ex.Message, "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Definições_Load(object sender, EventArgs e)
        {
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            // If load failed or was cancelled, remove any image (do not show ErrorImage)
            if (e.Error != null || e.Cancelled)
            {
                // Clear current image so nothing is shown
                pictureBox1.Image = null;

                // Also clear the ErrorImage so PictureBox won't draw it later (optional)
                pictureBox1.ErrorImage = null;
                return;
            }

        }

        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (var pen = new Pen(Color.Black, 2f)) // adjust color/width as needed
            {
                float half = pen.Width / 2f;
                float w = Math.Max(0, pictureBox1.ClientSize.Width - pen.Width);
                float h = Math.Max(0, pictureBox1.ClientSize.Height - pen.Width);
                e.Graphics.DrawEllipse(pen, half, half, w, h);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (comboBox1.SelectedValue != null && comboBox1.SelectedValue != DBNull.Value)
                {
                    string url = comboBox1.SelectedValue.ToString();
                    pictureBox1.LoadAsync(url);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
