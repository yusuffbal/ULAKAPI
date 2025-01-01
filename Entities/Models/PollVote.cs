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
    [Table("PollVotes")]
    public class PollVote : IEntity
    {
        [Key]
        public int PollVoteId { get; set; }

        public int PollOptionId { get; set; }
        public int VoterId { get; set; }

        public DateTime VoteTime { get; set; } = DateTime.UtcNow;

    }
}
