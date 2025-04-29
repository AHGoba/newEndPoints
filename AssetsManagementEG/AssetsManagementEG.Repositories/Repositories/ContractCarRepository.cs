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

        public Contract FindContract( int? contractId)
        {
            return context.Contract.FirstOrDefault(c=> c.ContractId == contractId); 
        }

        public ICollection<Contract> FindContracts(int id)
        {
            return context.Contract.Where(c => c.DistrictId == id).ToList();
        }

        // for Super User
        public List<Contract> GetContractsByDistricts(List<int> districtIds)
        {


            // Step 3: Get all contracts associated with the specified districts
            var contractsInDistricts = context.Contract
                .Where(c => districtIds.Contains(c.DistrictId) && c.EndDate> DateTime.Now) // Contracts in the specified districts
                .ToList();

            
            return contractsInDistricts;
        }
    }
}
