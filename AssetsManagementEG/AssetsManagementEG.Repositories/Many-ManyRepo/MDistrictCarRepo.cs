using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;

namespace AssetsManagementEG.Repositories.Many_ManyRepo
{
    public class MDistrictCarRepo : MGenericRepo<DistrictCar>
    {
        DBSContext context;
        public MDistrictCarRepo(DBSContext _context) : base(_context)
        {
            context = _context;
        }

        public IQueryable<DistrictCar> districts()
        {
            return context.DistrictCar;
        }
        public DistrictCar FindDistrictCar(int carId)
        {
            return context.DistrictCar.FirstOrDefault(c => c.CarId == carId);
        }
    }
}
