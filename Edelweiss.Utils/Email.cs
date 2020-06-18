using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Edelweiss.Utils
{
    public static class Email
    {
        #region Properties

        private static String SmtpHost { get; set; }
        private static Int32 SmtpPort { get; set; }
        private static String SmtpUser { get; set; }
        private static String SmtpPass { get; set; }
        private static Boolean SmtpEnableSSL { get; set; }
        private static Boolean UseDefaultCredentials { get; set; }
        private static String From { get; set; }
        private static List<String> To { get; set; }

        #endregion

        #region Static constructor

        static Email()
        {
            try
            {
                SmtpHost = ConfigurationManager.AppSettings["SMTP_HOST"];
                SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]);
                SmtpUser = ConfigurationManager.AppSettings["SMTP_USER"];
                SmtpPass = ConfigurationManager.AppSettings["SMTP_PASS"];
                SmtpEnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTP_ENABLE_SSL"]);
                UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["USE_DEFAULT_CREDENTIALS"]);
                From = ConfigurationManager.AppSettings["FROM"];
                To = ConfigurationManager.AppSettings["TO"].Split(';').ToList();
            }
            catch (Exception e)
            {
                Log.Create(e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Methods with short parametrs to sends an email to one or more informed email addresses, containing no or multiple attachments.
        /// </summary>
        /// <param name="to">List of addresses for</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body</param>
        /// <param name="attachments">Optional, List of attachments</param>
        public static void Send(List<String> to, String subject, String body, List<Attachment> attachments = null)
        {
            try
            {
                Validate(SmtpUser);
                Validate(From);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(From);

                foreach (String address in to)
                {
                    if (!String.IsNullOrEmpty(address))
                    {
                        Validate(address);
                        mail.To.Add(address);
                    }
                }

                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                mail.Body = Email.FormatBreakLineEmail(body);
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                if (attachments != null && attachments.Count > 0)
                {
                    foreach (Attachment attachment in attachments)
                    {
                        mail.Attachments.Add(attachment);
                    }
                }

                SmtpClient client = new SmtpClient(SmtpHost, SmtpPort);
                client.EnableSsl = SmtpEnableSSL;
                client.UseDefaultCredentials = UseDefaultCredentials;
                client.Credentials = new System.Net.NetworkCredential(SmtpUser, SmtpPass);
                client.Send(mail);
            }
            catch (Exception e)
            {
                Log.Create(e);
            }
        }

        /// <summary>
        /// Method with full options to sends an email to one or more informed email addresses, containing no or multiple attachments.
        /// </summary>
        /// <param name="smtpHost">E-mail service server</param>
        /// <param name="smtpUser">SMTP user</param>
        /// <param name="smtpPass">SMTP user password</param>
        /// <param name="from">Address of</param>
        /// <param name="to">List of addresses for</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body</param>
        /// <param name="smtpEnablesSSL">Optional, enables SSL</param>
        /// <param name="attachments">Optional, List of attachments</param>
        public static void Send(String smtpHost, Int32 smtpPort, String smtpUser, String smtpPass, String from, List<String> to, String subject, String body, Boolean smtpEnablesSSL = false, List<Attachment> attachments = null)
        {
            try
            {
                Validate(smtpUser);
                Validate(from);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);

                foreach (String address in to)
                {
                    if (!String.IsNullOrEmpty(address))
                    {
                        Validate(address);
                        mail.To.Add(address);
                    }
                }

                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                mail.Body = Email.FormatBreakLineEmail(body);
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                if (attachments != null && attachments.Count > 0)
                {
                    foreach (Attachment attachment in attachments)
                    {
                        mail.Attachments.Add(attachment);
                    }
                }

                SmtpClient client = new SmtpClient(smtpHost, smtpPort);
                client.EnableSsl = smtpEnablesSSL;
                client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
                client.Send(mail);
            }
            catch (Exception e)
            {
                Log.Create(e);
            }
        }

        /// <summary>
        /// Using the Email.Send method, send a mail with exception message.
        /// </summary>
        /// <param name="subject">A subject to quickly identification</param>
        /// <param name="e">The Exception object to get all informations of the exception.</param>
        public static void Send(String subject, Exception e)
        {
            String message = e.Message.Replace("\"", "").Replace("'", "");

            List<String> to = To;
            String body = Edelweiss.Utils.Properties.Resources.error_notification;
            body = body.Replace("\r\n", String.Empty);
            body = body.Replace("{{subject}}", subject);
            body = body.Replace("{{date-time}}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            body = body.Replace("{{hostname}}", Environment.MachineName);
            body = body.Replace("{{source}}", e.Source);
            body = body.Replace("{{message}}", message);
            body = body.Replace("{{inner-exception}}", e.InnerException != null ? e.InnerException.Message : "-");
            body = body.Replace("{{stack-trace}}", e.StackTrace);
            Email.Send(to, subject, body);
        }

        public static void Send(List<String> to, String subject, String body, EmailMessageType emailMessageType)
        {
            String formatBody = String.Empty;

            switch (emailMessageType)
            {
                case EmailMessageType.ERROR:
                    formatBody = Edelweiss.Utils.Properties.Resources.error_notification;
                    break;

                case EmailMessageType.SUCCESS:
                    formatBody = Edelweiss.Utils.Properties.Resources.success_notification;
                    break;

                case EmailMessageType.INFORMATION:
                    formatBody = Edelweiss.Utils.Properties.Resources.information_notification;
                    break;

                case EmailMessageType.WARNING:
                    formatBody = Edelweiss.Utils.Properties.Resources.warning_notification;
                    break;

                default:
                    formatBody = Edelweiss.Utils.Properties.Resources.information_notification;
                    break;
            }

            formatBody = formatBody.Replace("\r\n", String.Empty);
            formatBody = formatBody.Replace("{{subject}}", subject);
            formatBody = formatBody.Replace("{{message-body}}", body);
            Email.Send(to, subject, formatBody);
        }

        #endregion

        #region Auxiliary methods

        private static String FormatBreakLineEmail(String body)
        {
            body = body.Replace("\r\n", "<br>");
            return body;
        }

        public static void Validate(string email)
        {
            if (!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                throw new Exception(String.Format("Email {0} is not in a valid format.", email));
        }

        #endregion
    }

    public enum EmailMessageType
    {
        ERROR
        , SUCCESS
        , INFORMATION
        , WARNING
    }
}
