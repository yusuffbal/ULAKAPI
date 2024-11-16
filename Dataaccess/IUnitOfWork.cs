using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataaccess
{
    public interface IUnitOfWork
    {
        Task CommmitAsync();
        void Commit();
    }
}
