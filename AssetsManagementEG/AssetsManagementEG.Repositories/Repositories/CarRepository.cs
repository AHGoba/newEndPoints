using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Repositories.Repositories
{
    public class CarRepository : GenericRepository<Car>
    {
        DBSContext context;
        public CarRepository(DBSContext _context) : base(_context)
        {
            context = _context;
        }



    }
}
