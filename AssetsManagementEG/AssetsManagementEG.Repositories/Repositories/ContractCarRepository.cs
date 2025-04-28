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
            // Step 1: Get all CarIds related to these Districts
            var carIds = context.DistrictCar
                .Where(dc => districtIds.Contains(dc.DistrictId))
                .Select(dc => dc.CarId)
                .Distinct()
                .ToList();

            if (!carIds.Any())
            {
                return new List<Contract>(); // رجع List فاضية لو مفيش عربيات
            }

            // Step 2: Get all Contract IDs related to these Cars
            var contractIds = context.ContractsCars
                .Where(cc => carIds.Contains(cc.CarId))
                .Select(cc => cc.ContractId)
                .Distinct()
                .ToList();

            if (!contractIds.Any())
            {
                return new List<Contract>(); // رجع List فاضية لو مفيش عقود
            }

            // Step 3: Get the contracts
            var contracts = context.Contract
                .Where(c => contractIds.Contains(c.ContractId))
                .ToList();

            return contracts;
        }
    }
}
