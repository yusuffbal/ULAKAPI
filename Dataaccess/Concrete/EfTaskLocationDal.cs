using Dataaccess.Abstract;
using Dataaccess.Repositories;
using Entities.Models;

namespace Dataaccess.Concrete
{
    public class EfTaskLocationDal:EfEntityRepositoryBase<TaskLocation,AppDbContext>, ITaskLocationDal
    {
    }
}
