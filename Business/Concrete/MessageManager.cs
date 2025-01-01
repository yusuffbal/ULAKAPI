using Dataaccess.Abstract;
using Dataaccess;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Dataaccess.Concrete;
using Microsoft.Extensions.Configuration;
using Entities.Dtos;
using Business.Abstract;
using Business.Concrete;
using Azure.Messaging;

public class MessageManager : IMessageService
{
    private readonly IMessageDal _messageDal;
    private readonly IMessageGroupsDal _messageGroupsDal;
    private readonly IUsersDal _userDal;
    private readonly IUserTeamsDal _userTeamDal;
    private readonly ICryptingService _cryptingService;

    public MessageManager(IMessageDal messageDal, IMessageGroupsDal messageGroupsDal, IUsersDal usersDal, IUserTeamsDal userTeamsDal, ICryptingService cryptingService)
    {
        _messageDal = messageDal;
        _messageGroupsDal = messageGroupsDal;
        _userDal = usersDal;
        _userTeamDal = userTeamsDal;
        _cryptingService = cryptingService;
    }


    public void SendMessage(Message message)
    {
        // Yeni AES anahtarı oluşturuluyor
        var aesKey = _cryptingService.GenerateAesKey();

        // Mesaj AES ile şifreleniyor
        var encryptedMessage = _cryptingService.EncryptMessage(message.Content, aesKey);

        var senderPublicKey = _userDal.GetFirstOrDefault(x => x.UserId == message.SenderId).PublicKey;

        // Alıcının public key'i alınıyor
        var receiverPublicKey = _userDal.GetFirstOrDefault(x => x.UserId == message.RecipientId).PublicKey;

        // AES anahtarı RSA ile şifreleniyor
        var encryptedAesKey = _cryptingService.EncryptAesKey(aesKey, senderPublicKey, receiverPublicKey);

        // Şifrelenmiş mesaj ve AES anahtarı veritabanına kaydediliyor
        _messageDal.Add(new Message
        {
            SenderId = message.SenderId,
            RecipientId = message.RecipientId,
            Content = Convert.ToBase64String(encryptedMessage),  // Şifreli mesaj
            AesKey = Convert.ToBase64String(encryptedAesKey)  // Şifrelenmiş AES anahtarı
        });
    }


    public async Task<List<Message>> GetMessagesByUserIdAsync(int userId)
    {
        return _messageDal.GetList()
            .Where(m => m.SenderId == userId || m.RecipientId == userId).ToList();
    }

    public async Task<List<Message>> GetMessagesByGroupIdAsync(int groupId)
    {
        return  _messageDal.GetList()
            .Where(m => m.GroupId == groupId)
            .ToList();
    }

    public async Task<List<ChatListDto>> ListChatsAsync(int currentUserId)
    {
        try
        {
            // Kullanıcının private key'ini al
            var currentUserPrivateKey = _userDal.GetFirstOrDefault(u => u.UserId == currentUserId).EncryptedPrivateKey;

            // Bireysel sohbetleri almak için kullanıcı listesi
            var individualChats = _userDal.GetList()
                .Where(u => u.UserId != currentUserId)  // Sadece diğer kullanıcılar
                .Select(user =>
                {
                    // Son mesajı al
                    var lastMessage = _messageDal.GetList()
                        .Where(m =>
                            (m.SenderId == currentUserId && m.RecipientId == user.UserId) ||
                            (m.SenderId == user.UserId && m.RecipientId == currentUserId))
                        .OrderByDescending(m => m.SentAt)  // Son mesajı sırala
                        .FirstOrDefault();

                    // Varsayılan şifre çözülmüş mesaj
                    string decryptedMessage = "";

                    if (lastMessage != null)
                    {
                        try
                        {
                            // AES anahtarını deşifre et
                            var aesKey = _cryptingService.DecryptAesKey(
                                Convert.FromBase64String(lastMessage.AesKey),
                                _userDal.GetFirstOrDefault(u => u.UserId == lastMessage.SenderId).EncryptedPrivateKey,
                                _userDal.GetFirstOrDefault(u => u.UserId == lastMessage.RecipientId).EncryptedPrivateKey
                            );

                            // Mesajı deşifre et
                            decryptedMessage = _cryptingService.DecryptMessage(
                                Convert.FromBase64String(lastMessage.Content),
                                aesKey);
                        }
                        catch (Exception ex)
                        {
                            // Hata durumunda mesajı belirt
                            Console.WriteLine("Şifre çözme sırasında hata oluştu: " + ex.Message);
                            decryptedMessage = "Message decryption failed";
                        }
                    }

                    // Chat listesi için DTO oluştur
                    return new ChatListDto
                    {
                        ChatId = user.UserId,
                        MessageType = 0,  // Mesaj tipi 0 olarak varsayalım (örnek)
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        LastMessage = decryptedMessage,
                        LastMessageDate = lastMessage?.SentAt
                    };
                })
                .OrderByDescending(chat => chat.LastMessageDate)  // Mesaj tarihine göre sıralama
                .ToList();  // Listeye dönüştür

            // Geriye sohbet listesi döndür
            return individualChats;
        }
        catch (Exception ex)
        {
            // Genel hata durumunda loglama
            Console.WriteLine("Bir hata oluştu: " + ex.Message);
            return new List<ChatListDto>();  // Boş liste döndür
        }
    }




    public async Task<List<Message>> GetMessagesByChatAndUserIdAsync(int chatId, int userId)
    {
        try
        {
            // Alıcı (chatId) ve Gönderen (userId) arasında ki mesajları al
            var messages = _messageDal.GetList()
                .Where(m =>
                    (m.SenderId == userId && m.RecipientId == chatId) ||  // Gönderen: userId, Alıcı: chatId
                    (m.SenderId == chatId && m.RecipientId == userId))   // Gönderen: chatId, Alıcı: userId
                .OrderBy(m => m.SentAt)  // Mesajları tarih sırasına göre sırala
                .ToList();

            // Mesajları şifresiz hale getir
            foreach (var message in messages)
            {
                try
                {
                    // AES anahtarını deşifre et
                    var aesKey = _cryptingService.DecryptAesKey(
                        Convert.FromBase64String(message.AesKey),
                        _userDal.GetFirstOrDefault(u => u.UserId == message.SenderId).EncryptedPrivateKey,
                        _userDal.GetFirstOrDefault(u => u.UserId == message.RecipientId).EncryptedPrivateKey
                    );

                    // Mesajı deşifre et
                    message.Content = _cryptingService.DecryptMessage(
                        Convert.FromBase64String(message.Content),
                        aesKey);
                }
                catch (Exception ex)
                {
                    // Hata durumunda mesajı belirt
                    Console.WriteLine("Şifre çözme sırasında hata oluştu: " + ex.Message);
                    message.Content = "Message decryption failed"; // Şifre çözülemezse mesajı belirt
                }
            }

            // Geriye şifresi çözülmüş mesajları döndür
            return messages;
        }
        catch (Exception ex)
        {
            // Hata durumunda loglama
            Console.WriteLine("Bir hata oluştu: " + ex.Message);
            return new List<Message>();  // Boş liste döndür
        }
    }





}
