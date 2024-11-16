using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("Tasks")]
    public class Tasks:IEntity
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedByUserId { get; set; }
        public int? AssignedToUserId { get; set; }
        public int? AssignedToTeamId { get; set; }
        public int IsTeamTask { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Users AssignedByUser { get; set; }
        public virtual Users AssignedToUser { get; set; }
        public virtual Teams AssignedToTeam { get; set; }
    }
}
