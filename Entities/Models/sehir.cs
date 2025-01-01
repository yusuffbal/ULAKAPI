using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("sehir")]

    public class sehir :IEntity
    {
        [Key]
        public int Id { get; set; }
        public string SehirName { get; set; }
    }
}
