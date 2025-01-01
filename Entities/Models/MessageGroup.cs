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
    [Table("MessageGroups")]
    public class MessageGroup :IEntity
    {
        [Key]
        public int MessageGroupId { get; set; }

        [Required]
        [MaxLength(100)]
        public string GroupName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int TeamId { get; set; }

        // Navigation Properties
       
    }
}
