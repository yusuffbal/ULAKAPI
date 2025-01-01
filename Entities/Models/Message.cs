using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("Messages")]
    public class Message :IEntity
    {
        [Key]
        public int MessageId { get; set; }
        public string AesKey { get; set; }  

        public int SenderId { get; set; }
        public int? GroupId { get; set; }
        public int? RecipientId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

       
    }

}
