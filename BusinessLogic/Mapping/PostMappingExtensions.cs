using System.Runtime.CompilerServices;
using SocialMediaApp.DataAccess.Dtos.PostDto;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.BusinessLogic.Mapping
{
	public static class PostMappingExtensions
	{
		public static PostResponseDto ToPostResponseDto(this Post post)
		{
			var postResponseDto = new PostResponseDto
			{
				PostId = post.PostId,
				Question = post.Question,
				Answer = post.Answer,
				CreatedAt = post.CreatedAt,
				UserName = post.User.Username,
				CategoryName = post.Category.Name
			};
			return postResponseDto;
		}

		public static Post ToPost
	}
}
