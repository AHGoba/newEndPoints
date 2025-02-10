using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsManagementEG.Models.Models
{
    public class District
    {
        public int DistrictId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<DistrictCar> DistrictCar { get; set; }
        public virtual ICollection<DistrictEquibment> DistrictEquibment { get; set; }
        public virtual ICollection<DistrictLabors> DistrictLabors { get; set; }
        public virtual ICollection<Tassk> Task { get; set; }

        public virtual ICollection<UsersDistrict> UsersDistrict { get; set; }
    }
}
