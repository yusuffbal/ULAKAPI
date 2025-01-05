using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("UserTeams")]
    public class UserTeam : IEntity
    {
        [Key]
        public int UserTeamId { get; set; }

        public int UserId { get; set; }
        public int TeamId { get; set; }

        // Navigation Properties
      
    }
}
