using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class SendMessageDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Content { get; set; }
    }
}
