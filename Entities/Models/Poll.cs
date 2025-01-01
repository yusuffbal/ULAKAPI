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
    [Table("Polls")]
    public class Poll : IEntity
    {
        [Key]
        public int PollId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

    }
}
