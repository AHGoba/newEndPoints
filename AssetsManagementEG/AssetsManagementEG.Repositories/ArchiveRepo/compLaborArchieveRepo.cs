using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Models.Models.Archive;
using AssetsManagementEG.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetsManagementEG.Repositories.ArchiveRepo
{
    public class compLaborArchieveRepo : GenericRepository<CompLaborArchieve>
    {
        DBSContext context;
        public compLaborArchieveRepo(DBSContext _context) : base(_context)
        {
            context = _context;
        }

       
    }
}
