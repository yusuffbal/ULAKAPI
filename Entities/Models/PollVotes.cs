using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("PollVotes")]
    public class PollVotes:IEntity
    {

        public int PollVoteId { get; set; }
        public int PollOptionId { get; set; }
        public int VoterId { get; set; }
        public DateTime VoteTime { get; set; }

        public virtual PollOptions PollOption { get; set; }
        public virtual Users Voter { get; set; }
    }
}
