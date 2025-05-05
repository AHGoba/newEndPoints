using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public CompanyLabors FindCompanyLaborsRecord(int companyId)
        {
            return context.CompanyLabors.FirstOrDefault(d => d.ComapanyID == companyId);
        }

        public CompanyLabors FindCompanyLaborsRecordByLabor(int laborId)
        {
            return context.CompanyLabors.FirstOrDefault(d => d.LaborsID == laborId);
        }
    }
}
