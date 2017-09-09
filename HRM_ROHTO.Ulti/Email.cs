using System;
using System.Text;
using System.Web.Mail;

namespace HRM_ROHTO.Util
{
    public class Email
    {

        public static string _sendmail_from = "info@accountingvn.com";
        public static string _sendmail_email = "info@accountingvn.com";
        public static string _sendmail_password = "trathi722379";
        public static string _sendmail_serverName = "smtp.gmail.com";
        public static int _sendmail_smtpPort = 25;

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="from">From</param>
        /// <param name="to">To </param>
        /// <param name="bbc">Bbc</param>
        /// <param name="subject">Subject</param>
        /// <param name="messages">Containt</param>
        /// <param name="Files">File attacted</param>
        /// <returns></returns>
        public static bool SendMail(string to, string bbc, string subject, string messages, params string[] Files)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To = to;
                mail.Cc = bbc;
                mail.From = _sendmail_from;
                mail.Subject = subject;
                mail.BodyFormat = MailFormat.Html;

                if (Files.Length > 0)
                {
                    for (int i = 0; i < Files.Length; i++)
                    {
                        if (Files[i] == "")
                            continue;
                        mail.Attachments.Add(new MailAttachment(Files[i], MailEncoding.Base64));
                    }
                }
                mail.BodyEncoding = Encoding.GetEncoding("utf-8");
                mail.Body = messages;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"] = _sendmail_smtpPort;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 2;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"] = _sendmail_email;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"] = _sendmail_password;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"] = "true";
                //if (_sendmail_serverName == "")
                //{
                //    _sendmail_serverName = "smtp.gmail.com";
                //}
                SmtpMail.SmtpServer = _sendmail_serverName;
                SmtpMail.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
