using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VEGA.Models;

namespace VEGA.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VegaDbContext _context;

        public UnitOfWork(VegaDbContext _context)
        {
            this._context = _context;
        }

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }
    }
}
