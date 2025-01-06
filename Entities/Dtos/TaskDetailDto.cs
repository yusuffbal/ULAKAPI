using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class TaskDetailDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamName { get; set; }
        public int TaskId { get; set; }
        public string title { get; set; }
        public string status { get; set; }
        public string priority { get; set; }
        public string type { get; set; }
        public DateOnly DateOfStart { get; set; }
        public TimeOnly TimeOfStart { get; set; }
        public DateOnly DateOfEnd { get; set; }
        public string description { get; set; }
    }
}
