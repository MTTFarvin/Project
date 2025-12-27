using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Guna.UI2.WinForms;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Project
{
    public partial class OtpForm : Form
    {
        string userEmail;
        int timeLeft = 60;

        string conStr =
        "Data Source=COMPUTERADMINIS;Initial Catalog=SYTEM_POS;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        public OtpForm(string email)
        {
            InitializeComponent();
            userEmail = email;

            // OTP textbox auto move events
            txtOtp1.KeyUp += Otp_KeyUp;
            txtOtp2.KeyUp += Otp_KeyUp;
            txtOtp3.KeyUp += Otp_KeyUp;
            txtOtp4.KeyUp += Otp_KeyUp;
            txtOtp5.KeyUp += Otp_KeyUp;
            txtOtp6.KeyUp += Otp_KeyUp;

            // Max length = 1
            txtOtp1.MaxLength = 1;
            txtOtp2.MaxLength = 1;
            txtOtp3.MaxLength = 1;
            txtOtp4.MaxLength = 1;
            txtOtp5.MaxLength = 1;
            txtOtp6.MaxLength = 1;


            txtOtp1.TextChanged += TxtOtp1_TextChanged;
            txtOtp2.TextChanged += TxtOtp2_TextChanged;
            txtOtp3.TextChanged += TxtOtp3_TextChanged;
            txtOtp4.TextChanged += TxtOtp4_TextChanged;
            txtOtp5.TextChanged += TxtOtp5_TextChanged;
        }

        private void TxtOtp1_TextChanged(object sender, EventArgs e)
        {
            if (txtOtp1.Text.Length == 1)
                txtOtp2.Focus();
        }

        private void TxtOtp2_TextChanged(object sender, EventArgs e)
        {
            if (txtOtp2.Text.Length == 1)
                txtOtp3.Focus();
        }

        private void TxtOtp3_TextChanged(object sender, EventArgs e)
        {
            if (txtOtp3.Text.Length == 1)
                txtOtp4.Focus();
        }

        private void TxtOtp4_TextChanged(object sender, EventArgs e)
        {
            if (txtOtp4.Text.Length == 1)
                txtOtp5.Focus();
        }

        private void TxtOtp5_TextChanged(object sender, EventArgs e)
        {
            if (txtOtp5.Text.Length == 1)
                txtOtp6.Focus();
        }

        private void StartOtpTimer()
        {
            timeLeft = 60;
            lblTimer.Text = "01:00";
            lblResend.Enabled = false;
            otpTimer.Start();
        }


        // 🔹 Auto move method
        private void Otp_KeyUp(object sender, KeyEventArgs e)
        {
            Guna.UI2.WinForms.Guna2TextBox current =
        sender as Guna.UI2.WinForms.Guna2TextBox;

            if (current == null) return;

            if (e.KeyCode != Keys.Back && current.Text.Length == 1)
            {
                this.SelectNextControl(current, true, true, true, true);
            }
            else if (e.KeyCode == Keys.Back && current.Text.Length == 0)
            {
                this.SelectNextControl(current, false, true, true, true);
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            // Check empty
            if (txtOtp1.Text == "" || txtOtp2.Text == "" || txtOtp3.Text == "" ||
                txtOtp4.Text == "" || txtOtp5.Text == "" || txtOtp6.Text == "")
            {
                MessageBox.Show("Enter full OTP");
                return;
            }

            // Combine OTP
            string otp = txtOtp1.Text + txtOtp2.Text + txtOtp3.Text +
                         txtOtp4.Text + txtOtp5.Text + txtOtp6.Text;

            SqlConnection con = new SqlConnection(conStr);

            con.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM PasswordOTP WHERE Email=@e AND OTP=@o", con);

            cmd.Parameters.AddWithValue("@e", userEmail);
            cmd.Parameters.AddWithValue("@o", otp);

            int count = (int)cmd.ExecuteScalar();
            con.Close();

            if (count == 1)
            {
                NewPassword np = new NewPassword(userEmail);
                np.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid OTP");
            }
        }

        public void SendOtpMail(string toEmail, string otp)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new NetworkCredential("thasbiha0708@gmail.com", "nvcrznflrnpafudx");
                client.EnableSsl = true;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("thasbiha0708@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = "Your OTP Code";
                mail.Body = "Your OTP is: " + otp;

                client.Send(mail);
                MessageBox.Show("OTP sent to " + toEmail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending mail: " + ex.Message);
            }
        }

        private void lblResend_Click(object sender, EventArgs e)
        {
            txtOtp1.Clear();
            txtOtp2.Clear();
            txtOtp3.Clear();
            txtOtp4.Clear();
            txtOtp5.Clear();
            txtOtp6.Clear();
            txtOtp1.Focus();

            string newOtp = new Random().Next(100000, 999999).ToString();

            SqlConnection con = new SqlConnection(conStr);
                con.Open();

            SqlCommand del = new SqlCommand(
                "DELETE FROM PasswordOTP WHERE Email=@e", con);
            del.Parameters.AddWithValue("@e", userEmail);
            del.ExecuteNonQuery();

            SqlCommand ins = new SqlCommand(
                "INSERT INTO PasswordOTP (Email, OTP) VALUES (@e,@o)", con);
            ins.Parameters.AddWithValue("@e", userEmail);
            ins.Parameters.AddWithValue("@o", newOtp);
            ins.ExecuteNonQuery();

            con.Close();

            SendOtpMail(userEmail, newOtp);
            MessageBox.Show("New OTP sent to your email");

            // 🔥 ADD THESE 🔥
            lblResend.Enabled = false;
            StartOtpTimer();   // Timer restart (01:00)
        }

        private void OtpForm_Load(object sender, EventArgs e)
        {
            lblResend.Enabled = false;
            StartOtpTimer();   // first OTP timer start
        }

        private void lblTimer_Click(object sender, EventArgs e)
        {

        }

        private void otpTimer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;

                int min = timeLeft / 60;
                int sec = timeLeft % 60;

                lblTimer.Text = min.ToString("00") + ":" + sec.ToString("00");
            }
            else
            {
                otpTimer.Stop();
                lblTimer.Text = "00:00";
                lblResend.Enabled = true;

                MessageBox.Show("OTP Expired. Please resend OTP");
            }
        }
    }
    }

    

