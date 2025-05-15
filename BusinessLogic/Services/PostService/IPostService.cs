using SocialMediaApp.DataAccess.Dtos.PostDto;
using SocialMediaApp.DataAccess.Entity;

namespace SocialMediaApp.BusinessLogic.Services.PostService
{
    public interface IPostService
    {
        List<PostResponseDto> GetAll();
        PostResponseDto GetById(int id);
		List<PostResponseDto> GetByUser(int userId, int page, int pageSize);
		PostResponseDto Create(PostRequestDto postDto);
        PostResponseDto Update(int id, PostRequestDto updatedPostDto);
        void Delete(int id);
    }
}
