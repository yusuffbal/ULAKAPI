using Business.Abstract;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace ULAKAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly ICryptingService _cryptingService;

        public ChatHub(IMessageService messageService, ICryptingService cryptingService)
        {
            _messageService = messageService;
            _cryptingService = cryptingService;
        }

        public async Task SendMessageToUser(int recipientId, string messageContent)
        {
            if (int.TryParse(Context.UserIdentifier, out var senderId))
            {
                var encryptedMessage = _cryptingService.Encrypt(messageContent);

                await _messageService.AddMessage(senderId, recipientId, encryptedMessage);

                await Clients.User(recipientId.ToString()).SendAsync("ReceiveMessage", senderId, encryptedMessage, DateTime.UtcNow);
            }
            else
            {
                throw new Exception("Geçersiz senderId.");
            }
        }

        public async Task SendMessageToGroup(int groupId, string messageContent, string groupName)
        {
            if (int.TryParse(Context.UserIdentifier, out var senderId))
            {
                var encryptedMessage = _cryptingService.Encrypt(messageContent);

                await _messageService.AddGroupMessage(senderId, groupId, encryptedMessage);

                await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", senderId, encryptedMessage, DateTime.UtcNow);
            }
            else
            {
                throw new Exception("Geçersiz senderId.");
            }
        }

        public async Task<List<string>> GetMessages(int recipientId)
        {
            if (int.TryParse(Context.UserIdentifier, out var senderId))
            {
                var messages = await _messageService.GetMessages(senderId, recipientId);

                var decryptedMessages = new List<string>();
                foreach (var message in messages)
                {
                    decryptedMessages.Add(_cryptingService.Decrypt(message.Content));
                }

                return decryptedMessages;
            }
            else
            {
                throw new Exception("Geçersiz senderId.");
            }
        }

        public async Task<List<string>> GetGroupMessages(int groupId)
        {
            var messages = await _messageService.GetGroupMessages(groupId);

            var decryptedMessages = new List<string>();
            foreach (var message in messages)
            {
                decryptedMessages.Add(_cryptingService.Decrypt(message.Content));
            }

            return decryptedMessages;
        }

    }
}
