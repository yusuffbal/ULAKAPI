using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("Polls")]
    public class Polls:IEntity
    {
        public int PollId { get; set; }
        public string Title { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual Users CreatedByUser { get; set; }
        public virtual ICollection<PollOptions> PollOptions { get; set; }
    }
}
