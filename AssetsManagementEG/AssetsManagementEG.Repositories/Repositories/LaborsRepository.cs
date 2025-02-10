using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Repositories.Repositories
{
    public class LaborsRepository : GenericRepository<Labors>
    {
        DBSContext context;
        public LaborsRepository(DBSContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
