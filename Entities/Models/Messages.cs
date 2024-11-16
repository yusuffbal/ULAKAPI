using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("Messages")]
    public class Messages:IEntity
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int? GroupId { get; set; }
        public int? RecipientId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        public virtual Users Sender { get; set; }
        public virtual MessageGroups Group { get; set; }
        public virtual Users Recipient { get; set; }
    }
}
