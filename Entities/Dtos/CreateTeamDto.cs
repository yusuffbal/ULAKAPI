using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class CreateTeamDto
    {
        public int CreatedByUserId { get; set; }
        public string TeamName { get; set; }
    }
}
