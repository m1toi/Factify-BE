using SocialMediaApp.DataAccess.Dtos.PostDto;

namespace SocialMediaApp.BusinessLogic.Services.PostService
{
    public interface IPostService
    {
        List<PostResponseDto> GetAll();
        PostResponseDto GetById(int id);
        PostResponseDto Create(PostRequestDto postDto);
        PostResponseDto Update(int id, PostRequestDto updatedPostDto);
        void Delete(int id);
    }
}
