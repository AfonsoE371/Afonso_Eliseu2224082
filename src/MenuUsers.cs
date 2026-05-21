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
        int Id;
        string Username;
        string Email;
        string PasswordHash;

        public MenuUsers(string email, string passwordHash)
        {
            Email = email;
            PasswordHash = passwordHash;
        }

        public int GetUsername(string email, string passwordHash)
        {
            int value = 0;
            using (SqlConnection conn = new SqlConnection("Server = (localdb)\\MSSQLLocalDB; Database = YGOShopDB; Trusted_Connection = True"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Username, User_ID FROM Users WHERE Email = @Email AND PasswordHash = @Password", conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", passwordHash);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Username = reader["Username"] != DBNull.Value ? reader["Username"].ToString() : null;
                            Id = reader["User_ID"] != DBNull.Value ? Convert.ToInt32(reader["User_ID"]) : 0;
                            value = 1;
                        }
                        else
                        {

                            MessageBox.Show("Usuário não encontrado", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            return value;
        }

        public void register_image()
        {
            SqlConnection conn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Database=YGOShopDB;Trusted_Connection=True");
            {
                
                SqlCommand cmd = new SqlCommand("INSERT INTO UserProfileImage (User_ID, Image_ID) Values (@Idl, ",conn);
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        cmd


                    }
                    catch (SqlException ex)
                    {
                        
                    }
                }
            }
        }

    }
}
