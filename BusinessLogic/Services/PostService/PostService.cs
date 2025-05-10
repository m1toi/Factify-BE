using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.PostDto;
using SocialMediaApp.DataAccess.Repositories.PostRepository;
using SocialMediaApp.DataAccess.Repositories.UserRepository;

namespace SocialMediaApp.BusinessLogic.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public PostResponseDto Create(PostRequestDto postDto)
        {
			var post = postDto.ToPost();
			post.CreatedAt = DateTime.UtcNow;
			var createdPost = _postRepository.Create(post); 
			return createdPost.ToPostResponseDto();
		}

		public List<PostResponseDto> GetByUser(int userId)
		{
			var posts = _postRepository.GetByUser(userId);
			return posts.ToListPostResponseDto();
		}

		public void Delete(int id)
        {
            _postRepository.Delete(id);
        }

        public List<PostResponseDto> GetAll()
        {
            var posts = _postRepository.GetAll();
            return posts.ToListPostResponseDto();
        }

        public PostResponseDto GetById(int id)
        {
            var post = _postRepository.Get(id);
            return post.ToPostResponseDto();
        }


		public PostResponseDto Update(int id, PostRequestDto updatePostDto)
		{
			var existingPost = _postRepository.Get(id); 

			if (updatePostDto.UserId != existingPost.UserId)
			{
				throw new UnauthorizedAccessException("You are not authorized to edit this post");
			}
			
			existingPost.Question = updatePostDto.Question;
			existingPost.Answer = updatePostDto.Answer;
			existingPost.CategoryId = updatePostDto.CategoryId;
			var updatedPost = _postRepository.Update(id, existingPost); 

			return updatedPost.ToPostResponseDto(); 
		}
	}
}
