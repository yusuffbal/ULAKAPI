using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Filter
{
    public class MyTaskFilter
    {
        public int priority { get; set; }
        public int status { get; set; }
        public int taskId { get; set; }
        public string title { get; set; }
    }
}
