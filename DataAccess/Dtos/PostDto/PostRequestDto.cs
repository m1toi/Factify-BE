namespace SocialMediaApp.DataAccess.Dtos.PostDto
{
	public class PostRequestDto
	{
		public string Question { get; set; }
		public string Answer { get; set; }
		public int UserId { get; set; }
		public int CategoryId { get; set; }
	}
}
