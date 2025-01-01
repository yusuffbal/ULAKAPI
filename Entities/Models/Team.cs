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
    [Table("Teams")]
    public class Team : IEntity
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        [MaxLength(100)]
        public string TeamName { get; set; }

        public int CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties

    }
}
