using SocialMediaApp.DataAccess.Entity;

public class PasswordResetToken
{
	public int Id { get; set; }
	public string Token { get; set; } = null!;
	public int UserId { get; set; }
	public DateTime ExpirationUtc { get; set; }
	public bool Used { get; set; }

	public User User { get; set; } = null!;
}
