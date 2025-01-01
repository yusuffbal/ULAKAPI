using Entities.Dtos;
using Entities.Models;


public interface IMessageService
{

    void SendMessage(Message message);
    Task<List<Message>> GetMessagesByUserIdAsync(int userId);
    Task<List<Message>> GetMessagesByGroupIdAsync(int groupId);
    Task<List<ChatListDto>> ListChatsAsync(int currentUserId);


}
