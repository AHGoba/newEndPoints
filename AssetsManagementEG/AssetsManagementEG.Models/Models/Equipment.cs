using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsManagementEG.Models.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsInService { get; set; }
        public virtual ICollection<TaskEquipment> TaskEquipment { get; set; }
        public virtual ICollection<DistrictEquibment> DistrictEquibment { get; set; }
    }
}
