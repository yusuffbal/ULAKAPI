using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("Users")]
    public class Users : IEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PublicKey { get; set; }
        public string EncryptedPrivateKey { get; set; }

        public virtual ICollection<UserTeams> UserTeams { get; set; }
        public virtual ICollection<Task> AssignedTasks { get; set; }
        public virtual ICollection<Task> TasksAssignedTo { get; set; }
        public virtual ICollection<Messages> SentMessages { get; set; }
        public virtual ICollection<Messages> ReceivedMessages { get; set; }
        public virtual ICollection<Polls> CreatedPolls { get; set; }
        public virtual ICollection<PollVotes> PollVotes { get; set; }
    }
}
