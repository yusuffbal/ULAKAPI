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
    [Table("Tasks")]
    public class ProjectTask : IEntity
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        public int AssignedByUserId { get; set; }
        public int? AssignedToUserId { get; set; }
        public int? AssignedToTeamId { get; set; }

        [Required]
        public int IsTeamTask { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int TaskType { get; set; }
        public int TaskStatus { get; set; }
        public int TaskPriority { get; set; }

    
    }
}
