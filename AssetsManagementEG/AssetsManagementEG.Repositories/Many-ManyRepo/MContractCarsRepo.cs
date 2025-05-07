using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ContractsCars FindOneForUpdateOrDelete(int carId)
        {
            return context.ContractsCars
                .Include(cc=> cc.Contract)
                .Where(c=> c.CarId == carId)
                .OrderByDescending(c=> c.StartDate) // get latest contract related to this car 
                .FirstOrDefault(); // return only one record 
        }
    }
}
