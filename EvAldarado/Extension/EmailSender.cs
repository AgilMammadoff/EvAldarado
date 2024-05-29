using EvAldarado.Extension;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace EvAldarado.Extensions
{
    //public interface IEmailService
    //{
    //    Task SendEmailAsync(string to, string subject, string message);
    //    //Task SendEmailAsync(string to, string message);
    //}
    public class EmailService : IEmailService
    {
        private string? _host;
        private int _port;
        private bool _enableSSL;
        private string? _email;
        private string? _password;

        public EmailService(string host, int port, bool enableSSL, string? email, string? password)
        {
            _host = host;
            _port = port;
            _enableSSL = enableSSL;
            _email = email;
            _password = password;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var client = new System.Net.Mail.SmtpClient(_host, _port)
                {
                    Credentials = new NetworkCredential(_email, _password),
                    EnableSsl = _enableSSL
                };
                await client.SendMailAsync(new MailMessage(_email ?? "", email, subject, message) { IsBodyHtml = true });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine("Error sending email: " + ex.Message);

            }
        }


        //public class EmailSender : IEmailService
        //{
        //    private readonly IConfiguration _configuration;

        //    public EmailSender(IConfiguration configuration)
        //    {
        //        _configuration = configuration;
        //    }

        //    public async Task SendEmailAsync(string to, string subject, string message)
        //    {
        //        try
        //        {
        //            var emailSettings = _configuration.GetSection("EmailSettings");

        //            var mimeMessage = new MimeMessage();
        //            mimeMessage.From.Add(new MailboxAddress(emailSettings["FromName"], emailSettings["FromEmail"]));
        //            mimeMessage.To.Add(MailboxAddress.Parse(to));
        //            mimeMessage.Subject = subject;
        //            mimeMessage.Body = new TextPart("plain") { Text = message };

        //            using var client = new SmtpClient();
        //            await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), true);
        //            await client.AuthenticateAsync(emailSettings["FromEmail"], emailSettings["SmtpPassword"]);
        //            await client.SendAsync(mimeMessage);
        //            await client.DisconnectAsync(true);
        //        }
        //        catch(Exception ex)
        //        {
        //            Console.WriteLine($"Error:{ex}");
        //        }

        //    }



        //    public Task SendEmailAsync(string to, string message)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }

}