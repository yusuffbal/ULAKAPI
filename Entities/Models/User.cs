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
    [Table("Users")]
    public class User : IEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string PublicKey { get; set; }

        [Required]
        public string EncryptedPrivateKey { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        // Navigation Properties
      

    }
}
