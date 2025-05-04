using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.ConversationDto;
using SocialMediaApp.DataAccess.Repositories.ConversationRepository;

namespace SocialMediaApp.BusinessLogic.Services.ConversationService
{
	public class ConversationService : IConversationService
	{
		private readonly IConversationRepository _conversationRepository;

		public ConversationService(IConversationRepository conversationRepository)
		{
			_conversationRepository = conversationRepository;
		}

		public ConversationResponseDto GetConversation(int conversationId)
		{
			var conversation = _conversationRepository.GetConversation(conversationId);
			return conversation.ToConversationResponseDto();
		}

		public ConversationResponseDto GetConversationBetweenUsers(int user1Id, int user2Id)
		{
			var conversation = _conversationRepository.GetConversationBetweenUsers(user1Id, user2Id);
			return conversation?.ToConversationResponseDto();
		}

		public ConversationResponseDto CreateConversation(ConversationRequestDto conversationDto)
		{
			var conversationEntity = conversationDto.ToConversation();
			var createdConversation = _conversationRepository.Create(conversationEntity);
			return createdConversation.ToConversationResponseDto();
		}

		public List<ConversationResponseDto> GetUserConversations(int userId)
		{
			var conversations = _conversationRepository.GetUserConversations(userId);
			return conversations.ToListConversationResponseDto();
		}
	}
}
