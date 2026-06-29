using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YGOShop_AfonsoEliseu_2224082
{
    internal class MenuUsers
    {
        static int Id;
        static string Username;
        static string Email;
        static string PasswordHash;

        public void register(string email, string passwordHash)
        {
            Email = email;
            PasswordHash = passwordHash;
        }

        public void SHOW()
        {
            MessageBox.Show("Id: " + Id + "Username: " + Username);
        }

        public string returnUsername()
        { return Username; }

        public int returnID()
        { return Id; }


        public string GetUsernameBuy(int userID)
        {
            using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT Username FROM Users WHERE User_ID = @ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", userID);
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

        public int GetUsername(string email, string password)
        {
            int value = 0;


            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT Username, User_ID FROM Users WHERE Email = @Email AND PasswordHash = @Password", conn))
                    {

                        cmd.Parameters.AddWithValue("@Email", email.Trim());
                        cmd.Parameters.AddWithValue("@Password", password.Trim());

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Username = reader["Username"].ToString();


                                Id = Convert.ToInt32(reader["User_ID"]);

                                value = 1;
                            }
                            else
                            {
                                value = 0;
                            }
                        }
                    }



                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Registration Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            return value;
        }

        public int register_image(int imageId)
        {
            if (Id == 0)
            {
                MessageBox.Show("User ID is not set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True"))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(
                        "IF EXISTS (SELECT 1 FROM UserProfileImage WHERE User_ID = @UserID)\r\n" +
                        "BEGIN\r\n" +
                        "    UPDATE UserProfileImage\r\n" +
                        "    SET Image_ID = @ImageID\r\n" +
                        "    WHERE User_ID = @UserID\r\n" +
                        "END\r\n" +
                        "ELSE\r\n" +
                        "BEGIN\r\n" +
                        "    INSERT INTO UserProfileImage (User_ID, Image_ID)\r\n" +
                        "    VALUES (@UserID, @ImageID)\r\n" +
                        "END\r\n", conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", Id);
                        cmd.Parameters.AddWithValue("@ImageID", imageId);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Image registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return 1;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }




    }
}
