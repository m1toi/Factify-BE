using System.Net.Mail;
using System.Net;

namespace SocialMediaApp.BusinessLogic.Services.EmailSender
{
	public class SmtpEmailSender : IEmailSender
	{
		private readonly IConfiguration _cfg;
		public SmtpEmailSender(IConfiguration cfg) => _cfg = cfg;

		public async Task SendEmailAsync(string to, string subject, string htmlBody)
		{
			var emailCfg = _cfg.GetSection("Email");
			using var client = new SmtpClient(emailCfg["SmtpHost"], int.Parse(emailCfg["Port"]!))
			{
				Credentials = new NetworkCredential(emailCfg["User"]!, emailCfg["Password"]!),
				EnableSsl = true
			};

			var mail = new MailMessage(emailCfg["User"]!, to, subject, htmlBody) { IsBodyHtml = true };
			await client.SendMailAsync(mail);
		}
	}

}
