using Dataaccess.Abstract;
using Dataaccess.Repositories;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataaccess.Concrete
{
    public class EfMessageDal: EfEntityRepositoryBase<Message, AppDbContext>, IMessageDal
    {
    

      
    }
}
