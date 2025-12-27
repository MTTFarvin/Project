using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace Project
{

   
    public partial class ForgetPasswordForm : Form
    {
        
        public ForgetPasswordForm()
        {
            InitializeComponent();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Btnconfirm_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=COMPUTERADMINIS;Initial Catalog=SYTEM_POS;Integrated Security=True;Encrypt=False;TrustServerCertificate=True");
            String email = txtEmail.Text.Trim();

            if (email == "")
            {
                MessageBox.Show("Enter Email");
                return;
            }

            string otp = new Random().Next(100000, 999999).ToString();

            con.Open();

            SqlCommand cmd = new SqlCommand( "INSERT INTO PasswordOTP (Email, OTP, ExpiryTime) VALUES (@e,@o,GETDATE())", con);
            cmd.Parameters.AddWithValue("@e", email);
            cmd.Parameters.AddWithValue("@o", otp);
            cmd.ExecuteNonQuery();

            con.Close();

            SendOTP(email, otp);

            OtpForm f = new OtpForm(email);
            f.Show();
            this.Hide();
        }

        void SendOTP(string toMail, string otp)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("thasbiha0708@gmail.com");
            mail.To.Add(toMail);
            mail.Subject = "Password Reset OTP";
            mail.Body = "Your OTP is : " + otp; 

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(
                "thasbiha0708@gmail.com", "nvcrznflrnpafudx");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }

    }


    

