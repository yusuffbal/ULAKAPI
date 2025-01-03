using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Filter
{
    public class DashboardMapFilter
    {
        public int[] CityId { get; set; }
        public int TeamId { get; set; }
        public int TaskStatus { get; set; }
        public int TaskType { get; set; }
    }
}
