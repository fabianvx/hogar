using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

public class ServiceEmail
{
    private readonly string _smtpServer = "smtp.gmail.com"; // Para Gmail
    private readonly int _smtpPort = 587;
    private readonly string _smtpUser = "noreplyhogarjafethjimenez@gmail.com\r\n";  
    private readonly string _smtpPass = "gvbh ueos xzyy pbmm\r\n"; // 

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Hogar de Ancianos Jafeth Jimenez", _smtpUser));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { TextBody = body };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_smtpServer, _smtpPort, false);
        await client.AuthenticateAsync(_smtpUser, _smtpPass);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
