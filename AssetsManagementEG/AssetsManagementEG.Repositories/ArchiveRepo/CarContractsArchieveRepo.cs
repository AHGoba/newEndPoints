using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Models.Models.Archive;
using AssetsManagementEG.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Repositories.ArchiveRepo
{
    public class CarContractsArchieveRepo : GenericRepository<CarContractsArchieve>
    {
        DBSContext context;
        public CarContractsArchieveRepo(DBSContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
