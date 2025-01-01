using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class ChatListDto
    {
        public int ChatId { get; set; } // Sohbet ID (Bireysel kullanıcı veya grup)
        public int MessageType { get; set; } // 0: Bireysel, 1: Grup

        // Bireysel sohbet için
        public string? FirstName { get; set; } // Karşıdaki kullanıcının adı
        public string? LastName { get; set; } // Karşıdaki kullanıcının soyadı

        // Grup sohbeti için
        public string? GroupName { get; set; } // Grup adı

        // Son mesaj bilgisi
        public string? LastMessage { get; set; } // Son gönderilen mesaj içeriği
        public DateTime? LastMessageDate { get; set; } // Son mesajın gönderilme zamanı
    }
}
