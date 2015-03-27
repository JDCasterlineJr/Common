using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Mail;

namespace Common
{
    /// <summary>
    /// Allows applications to send e-mail by using the <see cref="System.Net.Mail.SmtpClient"/>
    /// </summary>
    public class Emailer
    {
        /// <summary>
        /// Sends the specified message to an SMTP server for delivery.
        /// </summary>
        /// <param name="to">Address to send email to.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body text of the email.</param>
        /// <param name="isBodyHtml">Is the body of the email html?</param>
        /// <param name="attachments">A list of <see cref="System.Net.Mail.Attachment"/>.</param>
        public static void Send(string to, string subject, string body, bool isBodyHtml, List<Attachment> attachments = null)
        {
            using (var smtpClient = new SmtpClient())
            {
                if (smtpClient.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
                    smtpClient.EnableSsl = false;

                using (var mailMsg = new MailMessage())
                {
                    mailMsg.To.Add(to);
                    mailMsg.Subject = subject;
                    mailMsg.Body = body;
                    mailMsg.IsBodyHtml = isBodyHtml;

                    if (attachments != null)
                        foreach(var attachment in attachments)
                            mailMsg.Attachments.Add(attachment);

                    smtpClient.Send(mailMsg);
                }
            }
        }

        /// <summary>
        /// Sends the specified message to an SMTP server for delivery.
        /// </summary>
        /// <param name="message"><see cref="System.Net.Mail.MailMessage"/> to send.</param>
        public static void Send(MailMessage message)
        {
            using (var smtpClient = new SmtpClient())
            {
                if (smtpClient.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
                    smtpClient.EnableSsl = false;

                smtpClient.Send(message);
            }
        }

        /// <summary>
        /// Sends the specified message to an SMTP server for delivery as an asynchronous operation.
        /// </summary>
        /// <param name="to">Address to send email to.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body text of the email.</param>
        /// <param name="isBodyHtml">Is the body of the email html?</param>
        /// <param name="attachments">A list of <see cref="System.Net.Mail.Attachment"/>.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task SendAsync(string to, string subject, string body, bool isBodyHtml, List<Attachment> attachments = null)
        {
            using (var smtpClient = new SmtpClient())
            {
                if (smtpClient.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
                    smtpClient.EnableSsl = false;

                using (var mailMsg = new MailMessage())
                {
                    mailMsg.To.Add(to);
                    mailMsg.Subject = subject;
                    mailMsg.Body = body;
                    mailMsg.IsBodyHtml = isBodyHtml;

                    if (attachments != null)
                        foreach (var attachment in attachments)
                            mailMsg.Attachments.Add(attachment);

                    await smtpClient.SendMailAsync(mailMsg);
                }
            }
        }

        /// <summary>
        /// Sends the specified message to an SMTP server for delivery as an asynchronous operation.
        /// </summary>
        /// <param name="message"><see cref="System.Net.Mail.MailMessage"/> to send.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task SendAsync(MailMessage message)
        {
            using (var smtpClient = new SmtpClient())
            {
                if (smtpClient.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
                    smtpClient.EnableSsl = false;

                await smtpClient.SendMailAsync(message);
            }
        }
    }
}
