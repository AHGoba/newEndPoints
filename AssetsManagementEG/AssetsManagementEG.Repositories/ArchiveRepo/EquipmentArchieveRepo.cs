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
    public class EquipmentArchieveRepo : GenericRepository<EquipmentArchieve>
    {
        DBSContext context;
        public EquipmentArchieveRepo(DBSContext _context) : base(_context)
        {
            context = _context;
        }

       
    }
}
