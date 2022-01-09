

using System.IO;
using System.Net.Mail;

namespace Fiorella_second.Utilities.File
{
    public class Helper
    {
        public static void RemoveFile(string root, string folder, string image)
        {
            string path = Path.Combine(root, folder, image);
            if (System.IO.File.Exists("path"))
            {
                System.IO.File.Delete(path);
            }
        }
        public enum UserRoles
        {
            SuperAdmin,
            Admin,
            Member,
            Moderator

        }
        public static class Email
        {
            public static void SendEmail(string fromEmail,string toEmail ,string body,string password ,string subject)
            {
                using (var client = new SmtpClient("smtp.googlemail.com", 587))
                {
                    client.Credentials =
                        new System.Net.NetworkCredential(fromEmail, password);
                    client.EnableSsl = true;
                    var msg = new MailMessage(fromEmail, toEmail);
                    msg.Body = body;
                    msg.Subject = subject;

                    client.Send(msg);
                }
            }
        }
    }
}
