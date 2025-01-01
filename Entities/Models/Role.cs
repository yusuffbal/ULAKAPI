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
    [Table("Roles")]
    public class Role : IEntity
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(100)]
        public string RoleName { get; set; }

        public string Description { get; set; }
    }
}
