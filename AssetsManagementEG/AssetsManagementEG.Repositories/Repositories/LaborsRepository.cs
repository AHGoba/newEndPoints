using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public bool LaborsExists(string name)
        {
            return context.Labors.Any(d => d.FullName == name);
        }
        public Labors FindLabors(string name)
        {
            return context.Labors.FirstOrDefault(d => d.FullName == name);
        }
    }
}
