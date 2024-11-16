using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("PollOptions")]
    public class PollOptions:IEntity
    {
        public int PollOptionId { get; set; }
        public int PollId { get; set; }
        public string OptionText { get; set; }
        public int Votes { get; set; }

        public virtual Polls Poll { get; set; }
    }
}
