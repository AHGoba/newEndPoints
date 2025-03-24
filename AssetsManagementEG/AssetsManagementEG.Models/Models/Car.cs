using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string PlateNum2  { get; set; }
        public bool IsAvailable { get; set; }
        // تبع الشركه ولا ايجار
        public bool IsCompanyOwned { get; set; }
        // لو مثلا عربيه اتشالت ممسحهاش لازم تبقا موجوده عندى فى الداتا بيز عشان هى ارتبطت بعدد من التاسكات
        public bool IsInService { get; set; }
        public virtual ICollection<TaskCar> TaskCars { get; set; }
        public virtual ICollection<DistrictCar> DistrictCar { get; set; }
        public virtual ICollection<ContractsCars> ContractsCars { get; set; }

        [ForeignKey("CarContractors")]
        public int? CarContractorsId { get; set; } // Foreign key to Car
        public CarContractors CarContractors { get; set; }
    }
}
