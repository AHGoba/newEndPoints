using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AssetsManagementEG.Repositories.Repositories
{
    public class DistrictRepository : GenericRepository<District>
    {
        DBSContext context;
        public DistrictRepository(DBSContext _context) : base(_context) 
            {
                context = _context;
            }
        public bool DistrictExists(string name)
        {
            return context.District.Any(d => d.Name == name);
        }
        public IQueryable<District> districts()
        {
            return context.District;
        }
    }
}
