using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;

namespace AssetsManagementEG.Repositories.Many_ManyRepo
{
    public class MDistrictLaborsRepo : MGenericRepo<DistrictLabors>
    {
        DBSContext context;
        public MDistrictLaborsRepo(DBSContext _context) : base(_context)
        {
            context = _context;
        }

        public IQueryable<DistrictLabors> districts()
        {
            return context.DistrictLabors;
        }
    }
}
