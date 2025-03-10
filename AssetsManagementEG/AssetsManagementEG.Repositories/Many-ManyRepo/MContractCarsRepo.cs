using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Repositories.Many_ManyRepo
{
    public class MContractCarsRepo : MGenericRepo<ContractsCars>
    {
        DBSContext context;
        public MContractCarsRepo(DBSContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
