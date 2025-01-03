using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class AddTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AssignedToUserId { get; set; }
        public int AssignedByUserId { get; set; }
        public int AssignedToTeamId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int TaskStatus { get; set; }
        public int TaskPriority { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Address { get; set; }
        public string SelectedCity { get; set; }
        public string DateOfStart { get; set; }
        public string DateOfEnd { get; set; }
        public string TimeOfStart { get; set; }
        public int Hour { get; set; }
    }
}
