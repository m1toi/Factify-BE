using SocialMediaApp.DataAccess.Dtos.ConversationDto;

namespace SocialMediaApp.BusinessLogic.Services.ConversationService
{
	public interface IConversationService
	{
		ConversationResponseDto GetConversation(int conversationId);
		List<ParticipantDto> GetConversationParticipants(int conversationId);
		ConversationResponseDto GetConversationBetweenUsers(int user1Id, int user2Id);
		ConversationResponseDto CreateConversation(ConversationRequestDto conversationDto);
		List<ConversationResponseDto> GetUserConversations(int userId);
	}
}
