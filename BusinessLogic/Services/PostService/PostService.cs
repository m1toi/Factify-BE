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
            _postRepository.Create(post);
            return post.ToPostResponseDto();
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

        public PostResponseDto Update(int id, PostRequestDto updatedPostDto)
        {
            var post = updatedPostDto.ToPost();
            post.PostId = id;
            _postRepository.Update(id, post);
            return post.ToPostResponseDto();
        }
    }
}
