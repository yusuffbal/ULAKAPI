using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class TeamListDto
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public string TeamCreatorName { get; set; }
        public string TeamCreatorLastName { get; set; }
        public string Status { get; set; } = "Aktif";

    }
}
