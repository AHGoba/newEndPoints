using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetsManagementEG.Repositories.Repositories
{
    public class CompanyLRepository : GenericRepository<CompanyL>
    {
        DBSContext context;
        public CompanyLRepository(DBSContext _context) : base(_context)
        {
            context = _context;
        }

        public bool CompanyLExists(string companyName)
        {
            return context.CompanyL.Any(c=> c.Name==companyName);
        }
    }
}
