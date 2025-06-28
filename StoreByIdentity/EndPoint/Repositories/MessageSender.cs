
using Azure.Identity;
using System.Net;
using System.Net.Mail;

namespace EndPoint.Repositories
{
    public class MessageSender : IMessageSender
    {
        public Task sendEmailAsync(string Toemail, string subject, string message, bool isMessageHtml = false)
        {
            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential()
                {
                    UserName="ahmadhossainpour",//without @gmail.com
                    Password="Ahmad@13506642"
                };
                client.Credentials = credentials;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                using var emailMessage = new MailMessage()
                {
                    To = { new MailAddress(Toemail)},
                    From= new MailAddress(""),   //with @gmail.com
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = isMessageHtml
                                               
                };
                client.Send(emailMessage);
            }
            return Task.CompletedTask;  
        }
    }
}
