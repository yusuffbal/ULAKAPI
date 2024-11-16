using Dataaccess.Abstract;
using Dataaccess;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

public class MessageManager : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageDal _messageDal;

    public MessageManager(IUnitOfWork unitOfWork, IMessageDal messageDal)
    {
        _unitOfWork = unitOfWork;
        _messageDal = messageDal;
    }

    public async Task AddMessage(int senderId, int recipientId, string messageContent)
    {
        var message = new Messages
        {
            SenderId = senderId,
            RecipientId = recipientId,
            Content = messageContent,
            SentAt = DateTime.UtcNow
        };

        _messageDal.Add(message);
        await _unitOfWork.CommmitAsync();
    }

    public async Task AddGroupMessage(int senderId, int groupId, string messageContent)
    {
        var message = new Messages
        {
            SenderId = senderId,
            GroupId = groupId,
            Content = messageContent,
            SentAt = DateTime.UtcNow
        };

        _messageDal.Add(message);
        await _unitOfWork.CommmitAsync();
    }

    public async Task<List<Messages>> GetMessages(int senderId, int recipientId)
    {
        return await _messageDal.GetListByExp(
                m => (m.SenderId == senderId && m.RecipientId == recipientId) ||
                     (m.SenderId == recipientId && m.RecipientId == senderId),
                orderBy: q => q.OrderBy(m => m.SentAt),
                include: q => q.Include(m => m.Sender).Include(m => m.Recipient)
            ).ToListAsync(); 
    }

    public async Task<List<Messages>> GetGroupMessages(int groupId)
    {
        return await _messageDal.GetListByExp(
                m => m.GroupId == groupId,
                orderBy: q => q.OrderBy(m => m.SentAt),
                include: q => q.Include(m => m.Sender).Include(m => m.Group)
            ).ToListAsync(); 
    }
}
