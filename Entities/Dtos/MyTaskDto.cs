using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class MyTaskDto
    {
        public string description { get; set; }
        public string status { get; set; }
        public string priority { get; set; }
        public string taskType { get; set; }

    }
}
