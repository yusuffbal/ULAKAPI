using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class SendMessageToGroupDto
    {
        public int SenderId { get; set; }
        public int GroupId { get; set; }
        public string Content { get; set; }
    }
}
