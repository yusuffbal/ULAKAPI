using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class TeamPersonDto
    {
        public string TeamName { get; set; }
        public string PersonName { get; set; }
        public string PersonLastName { get; set; }
        public string status { get; set; } = "Aktif";

    }
}
