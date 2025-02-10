using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsManagementEG.Models.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public string Type { get; set; }
        public string PlateNum { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsCompanyOwned { get; set; }
        public bool IsInService { get; set; }
        public virtual ICollection<TaskCar> TaskCars { get; set; }
        public virtual ICollection<DistrictCar> DistrictCar { get; set; }
    }
}
