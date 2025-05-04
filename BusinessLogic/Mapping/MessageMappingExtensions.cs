using SocialMediaApp.DataAccess.Dtos.MessageDto;
using SocialMediaApp.DataAccess.Entity;

public static class MessageMappingExtensions
{
	public static Message ToMessage(this MessageRequestDto dto)
	{
		return new Message
		{
			ConversationId = dto.ConversationId,
			SenderId = dto.SenderId,
			Content = dto.Content,
			PostId = dto.PostId
		};
	}

	public static MessageResponseDto ToMessageResponseDto(this Message message)
	{
		return new MessageResponseDto
		{
			MessageId = message.MessageId,
			ConversationId = message.ConversationId,
			SenderId = message.SenderId,
			SenderUsername = message.Sender.Username,
			Content = message.Content,
			SentAt = message.SentAt,
			PostId = message.PostId
		};
	}

	public static List<MessageResponseDto> ToListMessageResponseDto(this List<Message> messages)
	{
		return messages.Select(m => m.ToMessageResponseDto()).ToList();
	}
}
