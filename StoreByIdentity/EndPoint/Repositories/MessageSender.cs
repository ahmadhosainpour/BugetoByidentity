
using Azure.Identity;
using System.Net;
using System.Net.Mail;

namespace EndPoint.Repositories
{
    public class MessageSender : IMessageSender
    {
        public Task sendEmailAsync(string Toemail, string subject, string message, bool isMessageHtml = false)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("ahmadhpour1366@gmail.com", "amwt kvhc wpet ywee");

                using var emailMessage = new MailMessage()
                {
                    From = new MailAddress("ahmadhpour1366@gmail.com"),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = isMessageHtml
                };
                emailMessage.To.Add(Toemail);

                client.Send(emailMessage);
            }
            return Task.CompletedTask;
        }
    }
        //{
        //    using (var client = new SmtpClient("smtp.gmail.com", 587))
        //    {
        //        var credentials = new NetworkCredential()
        //        {
        //            UserName = "ahmadhossainpour@gmail.com",//without @gmail.com
        //            Password = "Ahmad@13506642"
        //        };
        //        client.Credentials = credentials;
        //        client.Host = "smtp.gmail.com";
        //        client.Port = 587;
        //        client.EnableSsl = true;
        //        using var emailMessage = new MailMessage()
        //        {
        //            To = { new MailAddress(Toemail) },
        //            From = new MailAddress("ahmadhossainpour@gmail"),   //with @gmail.com
        //            Subject = subject,
        //            Body = message,
        //            IsBodyHtml = isMessageHtml

        //        };
        //        client.Send(emailMessage);
        //    }
        //    return Task.CompletedTask;  
        //}
    }

