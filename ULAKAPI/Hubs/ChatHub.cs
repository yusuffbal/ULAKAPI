using Entities.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ULAKAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        private static readonly Dictionary<string, string> _userConnections = new Dictionary<string, string>();

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task RegisterConnection(string userId)
        {
            var connectionId = Context.ConnectionId;

            if (!_userConnections.ContainsKey(userId))
            {
                _userConnections.Add(userId, connectionId);
            }
            else
            {
                _userConnections[userId] = connectionId;
            }

            Console.WriteLine("registerconn: "+ _userConnections);
            await Clients.Caller.SendAsync("Connected", userId);
        }

        public async Task SendMessageToUser(string senderId, string recipientId, string content)
        {
            try
            {
                bool hasRecipientConnection = _userConnections.TryGetValue(recipientId, out var connectionIdRecipient);
                bool hasSenderConnection = _userConnections.TryGetValue(senderId, out var connectionIdSender);

                var message = new Message
                {
                    SenderId = int.Parse(senderId),
                    RecipientId = int.Parse(recipientId),
                    Content = content,
                    SentAt = DateTime.Now
                };

                if (hasRecipientConnection && hasSenderConnection)
                {
                    Console.WriteLine($"Recipient Connection: {connectionIdRecipient}, Sender Connection: {connectionIdSender}");

                    await Clients.Client(connectionIdRecipient).SendAsync("ReceiveMessage", message);
                }
                else
                {
                    Console.WriteLine("Recipient or sender is not connected. Saving message to database.");

                    _messageService.SendMessage(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessageToUser: {ex.Message}");
                throw; 
            }
        }





        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;

            foreach (var key in _userConnections.Keys)
            {
                if (_userConnections[key] == connectionId)
                {
                    _userConnections.Remove(key);
                    break;
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
