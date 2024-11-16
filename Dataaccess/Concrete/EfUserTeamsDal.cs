using Dataaccess.Abstract;
using Dataaccess.Repositories;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataaccess.Concrete
{
    public class EfUserTeamsDal : EfEntityRepositoryBase<UserTeams, AppDbContext>, IUserTeamsDal
    {
    }
}
