using SocialMediaApp.DataAccess.Dtos.ConversationDto;

namespace SocialMediaApp.BusinessLogic.Services.ConversationService
{
	public interface IConversationService
	{
		ConversationResponseDto GetConversation(int conversationId,int currentUserId);
		List<ParticipantDto> GetConversationParticipants(int conversationId);
		ConversationResponseDto GetConversationBetweenUsers(int user1Id, int user2Id, int currentUserId);
		ConversationResponseDto CreateConversation(ConversationRequestDto conversationDto, int currentUserId);
		List<ConversationResponseDto> GetUserConversations(int userId);
		void MarkConversationAsRead(int conversationId, int userId);
	}
}
