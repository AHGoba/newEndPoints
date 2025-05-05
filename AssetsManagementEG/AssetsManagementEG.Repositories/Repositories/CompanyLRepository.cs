using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public CompanyL FindCompany(string companyName)
        {
            return context.CompanyL.FirstOrDefault(d => d.Name == companyName);
        }

        public CompanyL FindCompanyById(int id)
        {
            return context.CompanyL.FirstOrDefault(c => c.CompanyID == id);
        }
    }
}
