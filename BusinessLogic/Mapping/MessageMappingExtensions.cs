using SocialMediaApp.DataAccess.Dtos.MessageDto;
using SocialMediaApp.DataAccess.Dtos.PostDto;
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
			SenderProfilePicture = message.Sender.ProfilePicture,
			Content = message.Content,
			SentAt = message.SentAt,
			PostId = message.PostId,
			Post = message.PostId.HasValue
			? new PostResponseDto
			{
				PostId = message.Post.PostId,
				Question = message.Post.Question,
				Answer = message.Post.Answer,
				CreatedAt = message.Post.CreatedAt,
				UserName = message.Post.User.Username,
				CategoryName = message.Post.Category.Name,
				UserId = message.Post.UserId,
				LikesCount = message.Post.Interactions?.Count(i => i.Liked) ?? 0,
				SharesCount = message.Post.Interactions?.Count(i => i.Shared) ?? 0,
			}
				: null
		};
	}

	public static List<MessageResponseDto> ToListMessageResponseDto(this List<Message> messages)
	{
		return messages.Select(m => m.ToMessageResponseDto()).ToList();
	}
}
