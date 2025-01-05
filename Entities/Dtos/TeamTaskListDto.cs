using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class TeamTaskListDto
    {
        public int TaskId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string status { get; set; }
        public string TaskType { get; set; }

    }
}
