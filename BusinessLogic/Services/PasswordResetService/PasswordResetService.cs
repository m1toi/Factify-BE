using Microsoft.EntityFrameworkCore;
using SocialMediaApp.BusinessLogic.Services.EmailSender;
using SocialMediaApp.DataAccess.DataContext;

namespace SocialMediaApp.BusinessLogic.Services.PasswordResetService
{
	public class PasswordResetService : IPasswordResetService
	{
		private readonly AppDbContext _context;
		private readonly IEmailSender _mailer;
		private readonly IConfiguration _cfg;

		public PasswordResetService(
			AppDbContext ctx,
			IEmailSender mailer,
			IConfiguration cfg)
		{
			_context = ctx;
			_mailer = mailer;
			_cfg = cfg;
		}

		public async Task RequestPasswordResetAsync(string email)
		{
			var user = _context.Users.SingleOrDefault(u => u.Email == email);
			// întotdeauna răspunzi generic mai jos, chiar dacă user e null

			var token = Guid.NewGuid().ToString("N");
			var expiry = DateTime.UtcNow.AddHours(1);

			if (user != null)
			{
				// marchează orice token vechi ca folosit
				var old = _context.PasswordResetTokens
							  .Where(t => t.UserId == user.UserId && !t.Used);
				foreach (var o in old) o.Used = true;

				_context.PasswordResetTokens.Add(new PasswordResetToken
				{
					UserId = user.UserId,
					Token = token,
					ExpirationUtc = expiry,
					Used = false
				});
				await _context.SaveChangesAsync();

				// trimite email
				var front = _cfg["Frontend:BaseUrl"]!.TrimEnd('/');
				var link = $"{front}/reset-password?token={token}";
				var html = $"<p>Click <a href=\"{link}\">aici</a> pentru a-ți reseta parola. Link-ul expiră în 1h.</p>";
				await _mailer.SendEmailAsync(email, "Resetare parolă Factify", html);
			}

			// răspuns generic, chiar dacă email-ul nu există
		}

		public async Task ResetPasswordAsync(string token, string newPassword)
		{
			var pr = _context.PasswordResetTokens
						 .Include(t => t.User)
						 .SingleOrDefault(t => t.Token == token);

			if (pr == null
			 || pr.Used
			 || pr.ExpirationUtc < DateTime.UtcNow)
				throw new Exception("Token invalid sau expirat.");

			// hash-uieşte parola
			pr.User.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
			pr.Used = true;
			_context.Users.Update(pr.User);
			await _context.SaveChangesAsync();
		}
	}

}
