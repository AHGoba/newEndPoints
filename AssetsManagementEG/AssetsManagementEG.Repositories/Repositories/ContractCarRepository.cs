using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetsManagementEG.Repositories.Repositories
{
    public class ContractCarRepository : GenericRepository<Contract>
    {
        DBSContext context;

        public ContractCarRepository(DBSContext _context) : base(_context)
        {
            context = _context;
        }
        public bool ContractCarExists(string contractName)
        {
            return context.Contract.Any(c=> c.ContractName == contractName);
        }

        public Contract FindContract( int contractId)
        {
            return context.Contract.FirstOrDefault(c=> c.ContractId == contractId); 
        }

        public ICollection<Contract> FindContracts(int id)
        {
            return context.Contract.Where(c => c.DistrictId == id).ToList();
        }
    }
}
