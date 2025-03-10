using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetsManagementEG.Repositories.Repositories
{
    public class CarRepository : GenericRepository<Car>
    {
        DBSContext context;
        public CarRepository(DBSContext _context) : base(_context)
        {
            context = _context;
        }

        public bool CarExists(string platenum)
        {
            return context.Car.Any(c=> c.PlateNum == platenum);
        }
        public Car FindCar(string platenum)
        {
            return context.Car.FirstOrDefault(c => c.PlateNum == platenum);
        }


    }
}
