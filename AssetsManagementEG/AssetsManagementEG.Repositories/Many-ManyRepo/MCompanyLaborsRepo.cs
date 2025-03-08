using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Repositories.Many_ManyRepo
{
    public class MCompanyLaborsRepo: MGenericRepo<CompanyLabors>
    {
        DBSContext context;
        public MCompanyLaborsRepo(DBSContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
