namespace SocialMediaApp.DataAccess.Dtos.PostDto
{
	public class PostResponseDto
	{
		public int PostId { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }
		public DateTime CreatedAt { get; set; }
		public string UserName { get; set; }
		public string CategoryName { get; set; }
		public int UserId { get; set; }
		public int? LikesCount { get; set; }
		public int? SharesCount { get; set; }
	}
}
