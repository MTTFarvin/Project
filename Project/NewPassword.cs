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

            lblNewPassError.Visible = false;
            lblConfirmPassError.Visible = false;

        }

        private void btnUpdateMyPassword_Click(object sender, EventArgs e)
        {
            string newPass = txtNewPassword.Text.Trim();
            string confirmPass = txtConfirmPassword.Text.Trim();

            // Reset error labels
            lblNewPassError.Visible = false;
            lblConfirmPassError.Visible = false;

            // 1️⃣ Empty check
            if (string.IsNullOrEmpty(newPass))
            {
                lblNewPassError.Text = "New password cannot be empty";
                lblNewPassError.Visible = true;
                return;
            }
            if (string.IsNullOrEmpty(confirmPass))
            {
                lblConfirmPassError.Text = "Confirm password cannot be empty";
                lblConfirmPassError.Visible = true;
                return;
            }

            // 2️⃣ Length check
            if (newPass.Length < 8)
            {
                lblNewPassError.Text = "Password must be at least 8 characters";
                lblNewPassError.Visible = true;
                return;
            }

            // 3️⃣ Match check
            if (newPass != confirmPass)
            {
                lblConfirmPassError.Text = "Passwords do not match";
                lblConfirmPassError.Visible = true;
                return;
            }

            try
            {
                con.Open();

                // 4️⃣ Check old password
                SqlCommand checkCmd = new SqlCommand(
                    "SELECT Password FROM Login WHERE Username=@Username", con);
                checkCmd.Parameters.AddWithValue("@Username", email);

                object oldPass = checkCmd.ExecuteScalar();

                if (oldPass != null && oldPass.ToString() == newPass)
                {
                    lblNewPassError.Text = "New password must be different from old password";
                    lblNewPassError.Visible = true;
                    return;
                }

                // 5️⃣ Update password
                SqlCommand updateCmd = new SqlCommand(
                    "UPDATE Login SET Password=@pass WHERE Username=@Username", con);
                updateCmd.Parameters.AddWithValue("@pass", newPass);
                updateCmd.Parameters.AddWithValue("@Username", email);

                updateCmd.ExecuteNonQuery();

                // ✅ Open login form immediately without any message
                Form1 login = new Form1();
                login.Show();
                this.Close(); // close current form
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

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
