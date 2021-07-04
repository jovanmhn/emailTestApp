using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace emailTestApp
{
    public partial class Form1 : Form
    {
        //static bool mailSent = false;
        public string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
                btnSend.Enabled = false;
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = textBoxServer.Text;
                smtpClient.Port = (int)numericUpDown.Value;
                smtpClient.EnableSsl = checkBoxSSL.Checked;
                smtpClient.Credentials = new System.Net.NetworkCredential(textBoxUsername.Text, textBoxPassword.Text);
                smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                MailMessage mail = new MailMessage();
                mail.Subject = "TEST APP";
                mail.Body = textBoxContent.Text;
                mail.From = new MailAddress(textBoxUsername.Text, "TEST APP", Encoding.UTF8);
                mail.To.Add(new MailAddress(textBoxRecipient.Text));
                string token = "token";
                smtpClient.SendAsync(mail, token);
                //textBoxResponse.Text += $"{DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss")} - Message sent." + System.Environment.NewLine + System.Environment.NewLine;
            }
            catch (Exception ex)
            {
                textBoxResponse.Text += "["+DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss")+"] " + $"ERROR {ex.GetType().ToString()}:" + ex.Message + (ex.InnerException?.Message ?? "") + System.Environment.NewLine + System.Environment.NewLine;
                btnSend.Enabled = true;
            }
        }

        private void checkBoxShow_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !checkBoxShow.Checked;
        }
        public void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                textBoxResponse.Text +=  String.Format("[{0}] Send canceled.", token) + System.Environment.NewLine + System.Environment.NewLine;
                btnSend.Enabled = true;
            }
            if (e.Error != null)
            {
                textBoxResponse.Text +=  String.Format ("[{1} - ERROR: {0}]", e.Error.ToString(), DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss")) + System.Environment.NewLine + System.Environment.NewLine;
                btnSend.Enabled = true;
            }
            else
            {
                textBoxResponse.Text +=  $"[{DateTime.Now.ToString("dd.MM.yyyy. HH:mm:ss")}] - Message sent." + System.Environment.NewLine + System.Environment.NewLine;
                btnSend.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxContent.Text += someArrows;
        }
    }
}
