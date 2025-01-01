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

        // Kullanıcı bağlantılarının tutulduğu statik bir sözlük
        private static readonly Dictionary<string, string> _userConnections = new Dictionary<string, string>();

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // Kullanıcı bağlantı kurduğunda çağrılır
        public async Task RegisterConnection(string userId)
        {
            var connectionId = Context.ConnectionId;

            // Kullanıcı daha önce bağlanmamışsa, yeni bağlantı ekle
            if (!_userConnections.ContainsKey(userId))
            {
                _userConnections.Add(userId, connectionId);
            }
            else
            {
                // Kullanıcı zaten bağlanmışsa, mevcut connectionId'yi güncelle
                _userConnections[userId] = connectionId;
            }

            // Bağlantının başarılı olduğunu istemciye bildir
            Console.WriteLine("registerconn: "+ _userConnections);
            await Clients.Caller.SendAsync("Connected", userId);
        }

        // Kullanıcıya mesaj gönder
        public async Task SendMessageToUser(string senderId, string recipientId, string content)
        {
            try
            {
                // Alıcının ve gönderenin bağlantı durumlarını kontrol et
                bool hasRecipientConnection = _userConnections.TryGetValue(recipientId, out var connectionIdRecipient);
                bool hasSenderConnection = _userConnections.TryGetValue(senderId, out var connectionIdSender);

                // Mesaj oluşturuluyor
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

                    // Mesajı alıcıya gönder
                    await Clients.Client(connectionIdRecipient).SendAsync("ReceiveMessage", message);
                }
                else
                {
                    Console.WriteLine("Recipient or sender is not connected. Saving message to database.");

                    // Eğer bağlantı kurulu değilse, mesajı veri tabanına kaydet
                    _messageService.SendMessage(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessageToUser: {ex.Message}");
                throw; // Hatanın istemciye iletilmesi gerekiyorsa bu satırı koruyabilirsiniz.
            }
        }



        // Gruplara mesaj gönder
        public async Task SendMessageToGroup(string groupId, string senderId, string content)
        {
            // Mesaj oluşturuluyor
            var message = new Message
            {
                SenderId = int.Parse(senderId),
                GroupId = int.Parse(groupId),
                Content = content,
                SentAt = DateTime.Now
            };

            // Mesajı veri tabanına kaydet
            _messageService.SendMessage(message);

            // Grupla mesajı paylaşıyoruz
            await Clients.Group(groupId).SendAsync("ReceiveMessage", message);
        }

        // Bağlantıyı sonlandırma (Bağlantı koparsa yapılacak işlem)
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;

            // Kullanıcı bağlantısını sonlandırıyoruz
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
