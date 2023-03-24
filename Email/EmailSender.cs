using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebShopMVC.Email
{
    public class EmailSender : IEmailSender
    {
        public async  Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(Options.SenGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("erhanaly@gmail.com", "YükselenShop"),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));
            try
            {
                await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception : {0}", ex.Message);
            }
        }
        public EmailOptions Options { get; set; }
        public EmailSender(IOptions<EmailOptions> emailOptions)
        {
            Options= emailOptions.Value;
        }  
    }
}
