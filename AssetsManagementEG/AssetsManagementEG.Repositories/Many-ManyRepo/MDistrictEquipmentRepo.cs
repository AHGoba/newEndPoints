using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;

namespace AssetsManagementEG.Repositories.Many_ManyRepo
{
    public class MDistrictEquipmentRepo : MGenericRepo<DistrictEquibment>
    {
        DBSContext context;
        public MDistrictEquipmentRepo(DBSContext _context) : base(_context)
        {
            context = _context;
        }

        public IQueryable<DistrictEquibment> districts()
        {
            return context.DistrictEquibment;
        }
    }
}
