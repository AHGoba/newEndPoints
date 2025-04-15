using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Repositories.ArchiveRepo
{
    public class CarArchiveRepo : GenericRepository<CarArchieve>
    {
        DBSContext context;
        public CarArchiveRepo(DBSContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
