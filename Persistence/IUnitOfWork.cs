using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VEGA.Persistance
{
    public interface IUnitOfWork
    {
        Task Complete();
    }
}
