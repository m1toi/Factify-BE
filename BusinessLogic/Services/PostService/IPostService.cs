using SocialMediaApp.DataAccess.Dtos.PostDto;

namespace SocialMediaApp.BusinessLogic.Services.PostService
{
    public interface IPostService
    {
        List<PostResponseDto> GetAll();
        PostResponseDto GetById(int id);
        PostResponseDto Create(PostRequestDto post);
        PostResponseDto Update(int id, PostRequestDto updatedPost);
        void Delete(int id);
    }
}
