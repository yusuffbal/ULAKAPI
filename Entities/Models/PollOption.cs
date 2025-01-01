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
    [Table("PollOptions")]
    public class PollOption : IEntity
    {
        [Key]
        public int PollOptionId { get; set; }

        public int PollId { get; set; }

        [Required]
        [MaxLength(200)]
        public string OptionText { get; set; }

        public int Votes { get; set; } = 0;

        // Navigation Property

    }
}
