using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsManagementEG.Models.Models
{
    public class DistrictCar
    {
        public int DistrictId { get; set; }
        public District District { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public DateTime StartDate { get; set; }


    }
}
