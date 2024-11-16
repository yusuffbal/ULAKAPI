using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("Teams")]
    public class Teams : IEntity
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Users CreatedByUser { get; set; }
        public virtual ICollection<UserTeams> UserTeams { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
        public virtual ICollection<MessageGroups> MessageGroups { get; set; }
    }
}
