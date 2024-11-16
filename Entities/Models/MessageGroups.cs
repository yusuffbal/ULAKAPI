using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("MessageGroups")]
    public class MessageGroups:IEntity
    {
        public int MessageGroupId { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TeamId { get; set; }
        public virtual Teams Team { get; set; }
        public virtual ICollection<Messages> Messages { get; set; }
    }
}
