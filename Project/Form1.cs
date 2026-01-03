using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace Project
{
    public partial class Form1 : Form
    {
        private ComponentResourceManager resManager;
        public static string LoggedUsername;

        public Form1()
        {
            InitializeComponent();

            // Localization resource manager
            resManager = new ComponentResourceManager(typeof(Form1));

            // Set form to full screen
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            
            comboBox1.SelectedIndex = 0; // default English
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

            // Initial language
            ApplyLanguage("en");
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lang = comboBox1.SelectedItem.ToString().ToLower();

            switch (lang)
            {
                case "ENGLISH": ApplyLanguage("en"); break;
                case "ARABIC": ApplyLanguage("ar"); break;
                case "BANGALA": ApplyLanguage("bn"); break;
                case "FRENCH": ApplyLanguage("fr"); break;
                case "RUSSIAN": ApplyLanguage("ru"); break;
                case "SPANISH": ApplyLanguage("es"); break;
            }
        }

        private void ApplyLanguage(string langCode)
        {
            // Apply language to thread
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(langCode);

            // Apply translated text to all controls
            ApplyResourceToControl(this, resManager);

            // Arabic RTL support
            if (langCode == "ar")
            {
                this.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = true;
            }
            else
            {
                this.RightToLeft = RightToLeft.No;
                this.RightToLeftLayout = false;
            }
        }

        private void ApplyResourceToControl(Control parent, ComponentResourceManager resource)
        {
            resource.ApplyResources(parent, parent.Name);

            foreach (Control c in parent.Controls)
            {
                resource.ApplyResources(c, c.Name);

                // apply recursively for nested controls
                if (c.Controls.Count > 0)
                    ApplyResourceToControl(c, resource);
            }
        }

        // LOGIN BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=COMPUTERADMINIS;Initial Catalog=SYTEM_POS;Integrated Security=True;Encrypt=False;TrustServerCertificate=True");

            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM Login WHERE Username = @username AND Password = @password", con);

            cmd.Parameters.AddWithValue("@username", txtUsername.Text);
            cmd.Parameters.AddWithValue("@password", txtPassword.Text);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                this.Hide();
                Form3 dsa = new Form3();
                dsa.Show();
            }
            else
            {
                MessageBox.Show("Wrong Username OR Password", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtUsername.Clear();
                txtPassword.Clear();
                txtUsername.Focus();
            }
            }
        

        // CLEAR & CLOSE BUTTON
        private void label4_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            this.Close();
        }

        // EXIT APPLICATION
        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // TEXTBOX CLEARING
        private void txtUsername_MouseClick(object sender, MouseEventArgs e)
        {
            txtUsername.Clear();
        }

        private void txtPassword_MouseClick(object sender, MouseEventArgs e)
        {
            txtPassword.Clear();
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            ForgetPasswordForm fp = new ForgetPasswordForm();
            fp.Show();
            this.Hide(); // login form hide
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}

