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
				CategoryName = post.Category.Name,
				UserId = post.UserId,
				LikesCount = post.Interactions?.Count(i => i.Liked) ?? 0,
				SharesCount = post.Interactions?.Count(i => i.Shared) ?? 0,
			};
			return postResponseDto;
		}

		public static Post ToPost(this PostRequestDto postRequestDto)
		{
			var post = new Post
			{
				Question = postRequestDto.Question,
				Answer = postRequestDto.Answer,
				UserId = postRequestDto.UserId,
				CategoryId = postRequestDto.CategoryId
			};
			return post;
		}

		public static List<PostResponseDto> ToListPostResponseDto(this List<Post> posts)
		{
			var postResponseDtos = new List<PostResponseDto>();
			foreach (var post in posts)
			{
				postResponseDtos.Add(post.ToPostResponseDto());
			}
			return postResponseDtos;	
		}
	}
}
