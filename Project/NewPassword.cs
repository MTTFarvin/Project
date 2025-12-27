using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Project
{
    public partial class NewPassword : Form
    {
        string email;
        SqlConnection con;

        public NewPassword(string mail)
        {
            InitializeComponent();
             email = mail;

            con = new SqlConnection("Data Source=COMPUTERADMINIS; Initial Catalog=SYTEM_POS;Encrypt=False;TrustServerCertificate=True");
            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;

        }

        private void btnUpdateMyPassword_Click(object sender, EventArgs e)
        {
            string newPass = txtNewPassword.Text.Trim();
            string confirmPass = txtConfirmPassword.Text.Trim();

            // 1️⃣ Empty check
            if (newPass == "" || confirmPass == "") 
            {
                MessageBox.Show("Password fields cannot be empty");
                return;
            }

            // 2️⃣ Length check
            if (newPass.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters");
                return;
            }

            // 3️⃣ Match check
            if (newPass != confirmPass)
            {
                MessageBox.Show("New Password and Confirm Password do not match");
                return;
            }

            try
            {
                con.Open();

                // 4️⃣ Old password same ah irukka check
                SqlCommand checkCmd = new SqlCommand(
                    "SELECT Password FROM Users WHERE Email=@email", con);
                checkCmd.Parameters.AddWithValue("@email", email);

                object oldPass = checkCmd.ExecuteScalar();

                if (oldPass != null && oldPass.ToString() == newPass)
                {
                    MessageBox.Show("New password must be different from old password");
                    return;
                }

                // 5️⃣ Update password
                SqlCommand updateCmd = new SqlCommand(
                    "UPDATE Users SET Password=@pass WHERE Email=@email", con);
                updateCmd.Parameters.AddWithValue("@pass", newPass);
                updateCmd.Parameters.AddWithValue("@email", email);

                updateCmd.ExecuteNonQuery();

                MessageBox.Show("Password updated successfully");

                // 6️⃣ Login form open
                Form1 login = new Form1();
                login.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
            finally
            {
                con.Close();
            }

        }  

        private void txtConfirmpassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void picEyeNew_Click(object sender, EventArgs e)
        {
           
            
        }

        private void picEyeConfirm_Click(object sender, EventArgs e)
        {
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            txtConfirmPassword.UseSystemPasswordChar = !txtConfirmPassword.UseSystemPasswordChar;
        }

        private void btnEyeNew_Click(object sender, EventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar = !txtNewPassword.UseSystemPasswordChar;
        }
    }
}
