using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Mail;
using System.IO;
using OfficeOpenXml;
using HRM_ROHTO.Models;
using HRM_ROHTO.Models.BUS;

namespace HRM_ROHTO.Controllers
{
    public class SendEmailController : Controller
    {
        //
        // GET: /SendEmail/
        public static string _sendmail_from = "info@accountingvn.com";
        public static string _sendmail_email = "info@accountingvn.com";
        public static string _sendmail_password = "trathi722379";
        public static string _sendmail_serverName = "smtp.gmail.com";
        public static int _sendmail_smtpPort = 25;

        public ActionResult Index()
        {
            string value = DA_Config.Instance.getValueFromKeyConfig("Email");
            _sendmail_serverName = DA_Config.Instance.getValueFromKeyConfig("ServerName");
            _sendmail_from = DA_Config.Instance.getValueFromKeyConfig("EmailServer");
            _sendmail_email = DA_Config.Instance.getValueFromKeyConfig("EmailServer");
            _sendmail_password = DA_Config.Instance.getValueFromKeyConfig("Password");
            _sendmail_smtpPort = Convert.ToInt32(DA_Config.Instance.getValueFromKeyConfig("SMTP"));
            Boolean UseSSL = DA_Config.Instance.getValueFromKeyConfig("UseSSLAdmin").ToString() == "1" ? true : false;

            string[] values = value.Split(';');
            DateTime now = DateTime.Now;
            string subject = "SỐ LƯỢNG CƠM LÝ THUYẾT (NGÀY " + (now.Day + 1) + "/" + now.Month + "/" + now.Year +")";
            try
            {
                ReportMealCardTomorrowController getFileNameReport = new ReportMealCardTomorrowController();
                ExcelPackage pck = new ExcelPackage();
                string refer = "";
                if (getFileNameReport.exportExcel(ref refer))
                {
                    string fileName = (Path.GetDirectoryName(Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)) + "/TK_SoLuongPhieuComDangKy.xlsx").Remove(0, 6);

                    string[] fileNames = new string[1] { fileName };
                    SendMail(_sendmail_serverName, _sendmail_from, _sendmail_password, _sendmail_smtpPort.ToString(), value, "", subject, "Vui lòng xem chi tiết file đính kèm.", UseSSL, fileNames);
                }
                else
                {
                    throw new Exception(refer);
                }
            }
            catch(Exception ex)
            {
                SendMail(_sendmail_serverName, _sendmail_from, _sendmail_password, _sendmail_smtpPort.ToString(), value, "", subject, "Xuất dữ liệu thất bại. Chi tiết lỗi " + ex.Message, UseSSL, new string[0]);
            }
            return View();
        }

        #region function send mail
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
        public static bool SendMail(string serverName, string from, string password, string smtpport, string to, string bbc, string subject, string messages, params string[] Files)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To = to;
                mail.Cc = bbc;
                mail.From = from;
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
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"] = smtpport;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 2;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"] = from;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"] = password;
                mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"] = "true";
                //if (_sendmail_serverName == "")
                //{
                //    _sendmail_serverName = "smtp.gmail.com";
                //}
                SmtpMail.SmtpServer = serverName;
                SmtpMail.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static bool SendMail(string serverName, string from, string password, string smtpport, string to, string bbc, string subject, string messages, bool UseSSL, params string[] Files)
        {
            try
            {
                MailMessage message = new MailMessage
                {
                    To = to,
                    Cc = bbc,
                    From = from,
                    Subject = subject,
                    BodyFormat = MailFormat.Html
                };
                if (Files.Length > 0)
                {
                    for (int i = 0; i < Files.Length; i++)
                    {
                        if (Files[i] != "")
                        {
                            message.Attachments.Add(new MailAttachment(Files[i], MailEncoding.Base64));
                        }
                    }
                }
                message.BodyEncoding = Encoding.GetEncoding("utf-8");
                message.Body = messages;
                message.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"] = smtpport;
                message.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 2;
                message.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
                message.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"] = from;
                message.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"] = password;
                message.Fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"] = UseSSL ? "true" : "false";
                SmtpMail.SmtpServer = serverName;
                SmtpMail.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

    }
}
