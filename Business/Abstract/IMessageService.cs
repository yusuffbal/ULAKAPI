using Entities.Models;


public interface IMessageService
{
    Task AddMessage(int senderId, int recipientId, string messageContent);
    Task AddGroupMessage(int senderId, int groupId, string messageContent);
    Task<List<Messages>> GetMessages(int senderId, int recipientId);
    Task<List<Messages>> GetGroupMessages(int groupId);
}
