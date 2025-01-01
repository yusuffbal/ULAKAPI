using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Models
{
    [Table("TaskLocation")]
    public class TaskLocation : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int TaskId { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }

        public decimal? Altitude { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Property
    }
}
