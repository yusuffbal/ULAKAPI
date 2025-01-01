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
            // Alıcı için connectionId bulunuyor
            if (_userConnections.TryGetValue(recipientId, out var connectionId))
            {
                Console.WriteLine(_userConnections);
                // Mesaj oluşturuluyor
                var message = new Message
                {
                    SenderId = int.Parse(senderId),
                    RecipientId = int.Parse(recipientId),
                    Content = content,
                    SentAt = DateTime.Now
                };
                _messageService.SendMessage(message);

                // Mesajı alıcıya gönder
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            }
            else
            {
                // Eğer alıcı bağlantı kurmamışsa, mesajı veri tabanına kaydedebiliriz
                var message = new Message
                {
                    SenderId = int.Parse(senderId),
                    RecipientId = int.Parse(recipientId),
                    Content = content,
                    SentAt = DateTime.Now
                };
                
                // Mesajı veri tabanına kaydet
                _messageService.SendMessage(message);
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
