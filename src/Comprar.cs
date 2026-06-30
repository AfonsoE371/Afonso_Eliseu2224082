using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace YGOShop_AfonsoEliseu_2224082
{
    public partial class Comprar : Form
    {
        public Comprar()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {

            if (e.CloseReason == CloseReason.UserClosing)
            {
                new Menu().Show();  
            }

            base.OnFormClosing(e);  
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int saleID = Convert.ToInt32(btn.Tag);
            BuyA frm = new BuyA(saleID);
            frm.Show();
            this.Hide();

        }


        private void Comprar_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True";

                string queryCards = "SELECT Card_ID FROM Cards WHERE 1=1";
                List<SqlParameter> cardParams = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    queryCards += " AND Nome LIKE @name";
                    cardParams.Add(new SqlParameter("@name", "%" + textBox1.Text + "%"));
                }

                List<string> catFilters = new List<string>();
                bool effectChecked = false;

                foreach (var item in checkedListBox1.CheckedItems)
                {
                    string v = item.ToString();
                    if (v == "Normal") catFilters.Add("Normal");
                    if (v == "Effect") { catFilters.Add("Effect"); effectChecked = true; }
                    if (v == "Fusion") catFilters.Add("Fusion");
                    if (v == "Ritual") catFilters.Add("Ritual");
                    if (v == "Synchro") catFilters.Add("Synchro");
                    if (v == "Pendulum") catFilters.Add("Pendulum");
                    if (v == "XYZ") catFilters.Add("XYZ");
                    if (v == "Link") catFilters.Add("Link");
                }

                foreach (string cat in catFilters)
                {
                    queryCards += " AND CardCategory LIKE @cat_" + cat;
                    cardParams.Add(new SqlParameter("@cat_" + cat, "%" + cat + "%"));
                }

                List<string> otherFilters = new List<string>();
                bool tunerChecked = false;

                foreach (var item in checkedListBox2.CheckedItems)
                {
                    string v = item.ToString();
                    if (v == "Toon") otherFilters.Add("Toon");
                    if (v == "Gemini") otherFilters.Add("Gemini");
                    if (v == "Union") otherFilters.Add("Union");
                    if (v == "Spirit") otherFilters.Add("Spirit");
                    if (v == "Flip") otherFilters.Add("Flip");
                    if (v == "Tuner") tunerChecked = true;
                }

                foreach (string o in otherFilters)
                {
                    queryCards += " AND MonsterType LIKE @other_" + o;
                    cardParams.Add(new SqlParameter("@other_" + o, "%" + o + "%"));
                }

                if (tunerChecked && !effectChecked)
                {
                    queryCards += " AND MonsterType = 'Tuner Monster'";
                }
                else if (effectChecked)
                {
                    queryCards += " AND (CardCategory LIKE '%Effect%' OR MonsterType = 'Tuner Monster')";
                }

                if (comboBox3.SelectedIndex > 0)
                {
                    queryCards += " AND MonsterType = @mtype";
                    cardParams.Add(new SqlParameter("@mtype", comboBox3.SelectedItem.ToString()));
                }

                if (comboBox8.SelectedIndex > 0)
                {
                    queryCards += " AND MonsterType = @stype";
                    cardParams.Add(new SqlParameter("@stype", comboBox8.SelectedItem.ToString()));
                }

                if (comboBox2.SelectedIndex > 0)
                {
                    queryCards += " AND Attribute = @attr";
                    cardParams.Add(new SqlParameter("@attr", comboBox2.SelectedItem.ToString()));
                }

                if (comboBox1.SelectedIndex > 0)
                {
                    queryCards += " AND Level = @level";
                    cardParams.Add(new SqlParameter("@level", comboBox1.SelectedItem.ToString()));
                }

                if (comboBox5.SelectedIndex > 0)
                {
                    queryCards += " AND Scales = @scale";
                    cardParams.Add(new SqlParameter("@scale", comboBox5.SelectedItem.ToString()));
                }

                if (int.TryParse(textBox3.Text, out int atk))
                {
                    queryCards += " AND Ataque >= @atk";
                    cardParams.Add(new SqlParameter("@atk", atk));
                }

                if (int.TryParse(textBox2.Text, out int def))
                {
                    queryCards += " AND Defesa >= @def";
                    cardParams.Add(new SqlParameter("@def", def));
                }

                DataTable cardTable = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(queryCards, conn))
                {
                    cmd.Parameters.AddRange(cardParams.ToArray());
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(cardTable);
                }

                List<int> cardIDs = new List<int>();

                foreach (DataRow r in cardTable.Rows)
                {
                    cardIDs.Add(Convert.ToInt32(r["Card_ID"]));
                }

                flowLayoutPanel1.Controls.Clear();

                if (cardIDs.Count == 0)
                {
                    Label lbl = new Label();
                    lbl.Text = "No items found";
                    lbl.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                    lbl.AutoSize = true;
                    lbl.Location = new Point(10, 10);
                    flowLayoutPanel1.Controls.Add(lbl);
                    return;
                }

                string ids = string.Join(",", cardIDs);
                MenuUsers menuUsers = new MenuUsers();  
                int currentUserID = menuUsers.returnID();

                string querySales = $@"
SELECT Vendas.Vendas_ID, Vendas.Card_ID, Vendas.Price, Vendas.Copies, 
       Users.Username, Cards.Nome, CardImages.Image_URL
FROM Vendas
JOIN Users ON Users.User_ID = Vendas.User_ID
JOIN Cards ON Cards.Card_ID = Vendas.Card_ID
JOIN CardImages ON CardImages.Card_ID = Cards.Card_ID
WHERE Vendas.User_ID <> @CurrentUserID
AND Vendas.Card_ID IN ({ids})"; ;




                DataTable salesTable = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(querySales, conn))
                {
                    conn.Open(); 
                    cmd.Parameters.AddWithValue("@CurrentUserID", currentUserID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(salesTable);
                }

                if (salesTable.Rows.Count == 0)
                {
                    Label lbl = new Label();
                    lbl.Text = "No items found";
                    lbl.Font = new Font("MatrixIIOT-Bold", 14, FontStyle.Bold);
                    lbl.AutoSize = true;
                    lbl.Location = new Point(10, 10);
                    flowLayoutPanel1.Controls.Add(lbl);
                    return;
                }

                foreach (DataRow row in salesTable.Rows)
                {
                    Panel card = new Panel();
                    card.Width = 280;
                    card.Height = 200;
                    card.BorderStyle = BorderStyle.FixedSingle;
                    card.Margin = new Padding(5);
                    card.BackColor = Color.White;

                    PictureBox pic = new PictureBox();
                    pic.Width = 100;
                    pic.Height = 140;
                    pic.SizeMode = PictureBoxSizeMode.Zoom;
                    if (row["Image_URL"] != DBNull.Value)
                    {
                        string url = row["Image_URL"].ToString();
                        using (WebClient wc = new WebClient())
                        {
                            byte[] data = wc.DownloadData(url);
                            using (MemoryStream ms = new MemoryStream(data))
                            {
                                pic.Image = Image.FromStream(ms);
                            }
                        }
                    }

                    Label lblName = new Label();
                    lblName.Text = "Card: " + row["Nome"].ToString();
                    lblName.AutoSize = true;
                    lblName.Location = new Point(120, 10);

                    Label lblSaleID = new Label();
                    lblSaleID.Text = "Sale ID: " + row["Vendas_ID"].ToString();
                    lblSaleID.AutoSize = true;
                    lblSaleID.Location = new Point(120, 40);

                    Label lblQty = new Label();
                    lblQty.Text = "Quantity: " + row["Copies"].ToString();
                    lblQty.AutoSize = true;
                    lblQty.Location = new Point(120, 60);

                    Label lblPrice = new Label();
                    lblPrice.Text = "Price: " + row["Price"].ToString() + "€";
                    lblPrice.AutoSize = true;
                    lblPrice.Location = new Point(120, 80);

                    Button btnOpen = new Button();
                    btnOpen.Text = "Open";
                    btnOpen.Width = 100;
                    btnOpen.Height = 30;
                    btnOpen.Location = new Point(120, 140);
                    btnOpen.Tag = row["Vendas_ID"];
                    btnOpen.Click += BtnOpen_Click;

                    card.Controls.Add(pic);
                    card.Controls.Add(lblName);
                    card.Controls.Add(lblSaleID);
                    card.Controls.Add(lblQty);
                    card.Controls.Add(lblPrice);
                    
                    card.Controls.Add(btnOpen);

                    flowLayoutPanel1.Controls.Add(card);
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
            this.Close();
            
        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
