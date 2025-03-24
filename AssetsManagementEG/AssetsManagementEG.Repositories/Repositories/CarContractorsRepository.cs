using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetsManagementEG.Repositories.Repositories
{
    public class CarContractorsRepository : GenericRepository<CarContractors>
    {
        DBSContext context;
        public CarContractorsRepository(DBSContext _context) : base (_context)
        {
            context = _context;
        }

        public bool CheckContractorsExcisting ( string name)
        {
            return context.CarContractors.Any (c => c.Name == name);
        }
    }
}
