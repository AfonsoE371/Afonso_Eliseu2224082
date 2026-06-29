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

namespace YGOShop_AfonsoEliseu_2224082
{


    public partial class Coleção : Form
    {
        private int currentUserID;

        public Coleção(int userID)
        {
            InitializeComponent();
            currentUserID = userID;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
        }

        private void Coleção_Load(object sender, EventArgs e)
        {
            LoadCollection();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int GetCardIDByName(string cardName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();
                    string query = "SELECT Card_ID FROM Cards WHERE Nome = @Nome";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nome", cardName);
                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : -1;
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
            return -1;                    
        }

        private void AddCardCopiesByName(string cardName, int amount)
        {
            int cardID = GetCardIDByName(cardName);
            if (cardID == -1) return;

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();

                    string check = @"
                SELECT Quantity
                FROM Collection
                WHERE User_ID = @U AND Card_ID = @C";

                    int quantity = 0;

                    using (SqlCommand cmd = new SqlCommand(check, conn))
                    {
                        cmd.Parameters.AddWithValue("@U", currentUserID);
                        cmd.Parameters.AddWithValue("@C", cardID);
                        object result = cmd.ExecuteScalar();
                        quantity = result != null ? Convert.ToInt32(result) : 0;
                    }

                    int newQuantity = quantity + amount;

                    if (quantity == 0)
                    {
                        using (SqlCommand insert = new SqlCommand(
                            "INSERT INTO Collection (User_ID, Card_ID, Quantity) VALUES (@U, @C, @Q)", conn))
                        {
                            insert.Parameters.AddWithValue("@U", currentUserID);
                            insert.Parameters.AddWithValue("@C", cardID);
                            insert.Parameters.AddWithValue("@Q", newQuantity);
                            insert.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (SqlCommand update = new SqlCommand(
                            "UPDATE Collection SET Quantity = @Q WHERE User_ID = @U AND Card_ID = @C", conn))
                        {
                            update.Parameters.AddWithValue("@Q", newQuantity);
                            update.Parameters.AddWithValue("@U", currentUserID);
                            update.Parameters.AddWithValue("@C", cardID);
                            update.ExecuteNonQuery();
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

        private void RemoveCardCopiesByName(string cardName, int amount)
        {
            using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
            {
                conn.Open();

                string check = @"
                SELECT Col.Quantity, Col.Card_ID
                FROM Collection Col
                JOIN Cards C ON Col.Card_ID = C.Card_ID
                WHERE Col.User_ID = @U AND C.Nome = @Nome";

                int quantity = 0;
                int cardID = -1;

                using (SqlCommand cmd = new SqlCommand(check, conn))
                {
                    cmd.Parameters.AddWithValue("@U", currentUserID);
                    cmd.Parameters.AddWithValue("@Nome", cardName);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read()) return;
                        quantity = Convert.ToInt32(reader["Quantity"]);
                        cardID = Convert.ToInt32(reader["Card_ID"]);
                    }
                }

                int newQuantity = quantity - amount;

                if (newQuantity <= 0)
                {
                    using (SqlCommand delete = new SqlCommand(
                        "DELETE FROM Collection WHERE User_ID = @U AND Card_ID = @C", conn))
                    {
                        delete.Parameters.AddWithValue("@U", currentUserID);
                        delete.Parameters.AddWithValue("@C", cardID);
                        delete.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlCommand update = new SqlCommand(
                        "UPDATE Collection SET Quantity = @Q WHERE User_ID = @U AND Card_ID = @C", conn))
                    {
                        update.Parameters.AddWithValue("@Q", newQuantity);
                        update.Parameters.AddWithValue("@U", currentUserID);
                        update.Parameters.AddWithValue("@C", cardID);
                        update.ExecuteNonQuery();
                    }
                }
            }
        }

        private Panel CreateCardPanel(string imageUrl, int copies)
        {
            Panel panel = new Panel();
            panel.Width = 180;
            panel.Height = 260;
            panel.Margin = new Padding(10);

            PictureBox pic = new PictureBox();
            pic.Dock = DockStyle.Fill;
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            if (!string.IsNullOrEmpty(imageUrl)) pic.ImageLocation = imageUrl;
            else pic.BackColor = Color.Black;

            Label lblCopies = new Label();
            lblCopies.Text = copies.ToString();
            lblCopies.BackColor = Color.FromArgb(180, 0, 0, 0);
            lblCopies.ForeColor = Color.White;
            lblCopies.Font = new Font("Arial", 12, FontStyle.Bold);
            lblCopies.AutoSize = true;
            lblCopies.Location = new Point(5, 5);

            panel.Controls.Add(pic);
            panel.Controls.Add(lblCopies);
            lblCopies.BringToFront();

            return panel;
        }

        private void LoadCollection()
        {
            flowLayoutPanel1.Controls.Clear();

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();

                    string query = @"
                SELECT C.Card_ID, C.Nome, CI.Image_URL, Col.Quantity,
                       C.CardCategory, C.MonsterType, C.Attribute, C.Level
                FROM Collection Col
                JOIN Cards C ON Col.Card_ID = C.Card_ID
                LEFT JOIN CardImages CI ON C.Card_ID = CI.Card_ID
                WHERE Col.User_ID = @UserID";

                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@UserID", currentUserID));

                    // Filtro por nome
                    if (!string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        query += " AND C.Nome LIKE @name";
                        parameters.Add(new SqlParameter("@name", "%" + textBox1.Text + "%"));
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
                        query += " AND C.CardCategory LIKE @cat_" + cat;
                        parameters.Add(new SqlParameter("@cat_" + cat, "%" + cat + "%"));
                    }


                    if (comboBox8.SelectedIndex > 0)
                    {
                        query += " AND C.MonsterType = @stype";
                        parameters.Add(new SqlParameter("@stype", comboBox8.SelectedItem.ToString()));
                    }


                    if (comboBox3.SelectedIndex > 0)
                    {
                        query += " AND C.MonsterType = @mtype";
                        parameters.Add(new SqlParameter("@mtype", comboBox3.SelectedItem.ToString()));
                    }


                    if (comboBox2.SelectedIndex > 0)
                    {
                        query += " AND C.Attribute = @attr";
                        parameters.Add(new SqlParameter("@attr", comboBox2.SelectedItem.ToString()));
                    }


                    if (comboBox1.SelectedIndex > 0)
                    {
                        query += " AND C.Level = @level";
                        parameters.Add(new SqlParameter("@level", comboBox1.SelectedItem.ToString()));
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        foreach (var p in parameters)
                            cmd.Parameters.Add(p);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string url = reader["Image_URL"] == DBNull.Value ? "" : reader["Image_URL"].ToString();
                                int quantity = Convert.ToInt32(reader["Quantity"]);
                                Panel cardPanel = CreateCardPanel(url, quantity);
                                flowLayoutPanel1.Controls.Add(cardPanel);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            LoadCollection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cardName = textBox2.Text.Trim();
            if (!int.TryParse(textBox3.Text, out int amount) || amount <= 0) return;
            AddCardCopiesByName(cardName, amount);
            LoadCollection();
        }


        private void button3_Click_1(object sender, EventArgs e)
        {
            string cardName = textBox2.Text.Trim();
            if (!int.TryParse(textBox3.Text, out int amount) || amount <= 0) return;
            RemoveCardCopiesByName(cardName, amount);
            LoadCollection();
        }
    }
}
