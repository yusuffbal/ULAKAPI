using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("UserTeams")]
    public class UserTeams : IEntity
    {
        public int UserTeamId { get; set; }
        public int UserId { get; set; }
        public int TeamId { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }

        public virtual Users User { get; set; }
        public virtual Teams Team { get; set; }
        public virtual Roles RoleDetails { get; set; }
    }
}
