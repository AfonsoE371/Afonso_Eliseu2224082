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
using static System.Net.Mime.MediaTypeNames;

namespace YGOShop_AfonsoEliseu_2224082
{
    public partial class Definições : Form
    {
        public string url;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!fecharForcado && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                return;
            }
        }
        private bool fecharForcado = false;
        public Definições()
        {

            InitializeComponent();
            Utilidades utilidades = new Utilidades();
            utilidades.arredondar(pictureBox1);

           
            pictureBox1.Paint += pictureBox1_Paint_1;

           
            pictureBox1.ErrorImage = null;
            pictureBox1.Image = null;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();


                    string query = @"
SELECT 
            C.Nome, 
            CI.Image_Cropped_URL,
            CI.Image_ID
        FROM Cards C 
        INNER JOIN CardImages CI ON C.Card_ID = CI.Card_ID";



                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "Nome";
                    comboBox1.ValueMember = "Image_ID";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Connection error with the database: " + ex.Message, "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has ocurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        public void Definições_Load(object sender, EventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {
            fecharForcado = true;
            this.Close();
            new Menu().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (comboBox1.SelectedValue != null)
            {

                int imageId = Convert.ToInt32(comboBox1.SelectedValue);

                DataRowView row = (DataRowView)comboBox1.SelectedItem;
                string url = row["Image_Cropped_URL"].ToString();

                MenuUsers menuUsers = new MenuUsers();
                int result = menuUsers.register_image(imageId);
            }
            else
            {
                MessageBox.Show("Select an image first.");
            }


        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            if (e.Error != null || e.Cancelled)
            {
                pictureBox1.Image = null;

            
                pictureBox1.ErrorImage = null;
                return;
            }

        }

        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (var pen = new Pen(Color.Black, 2f))
            {
                float half = pen.Width / 2f;
                float w = Math.Max(0, pictureBox1.ClientSize.Width - pen.Width);
                float h = Math.Max(0, pictureBox1.ClientSize.Height - pen.Width);
                e.Graphics.DrawEllipse(pen, half, half, w, h);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedItem != null)
            {
                DataRowView row = (DataRowView)comboBox1.SelectedItem;

                string url = row["Image_Cropped_URL"].ToString();

                int imageId = Convert.ToInt32(row["Image_ID"]);

                try
                {
                    pictureBox1.LoadAsync(url);
                }
                catch
                {
                    pictureBox1.Image = null;
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            fecharForcado = true;
            this.Close();
            new Login().Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Definições_Load_1(object sender, EventArgs e)
        {

        }
    }
}
