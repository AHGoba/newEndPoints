using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsManagementEG.Models.Models
{
    public class DistrictEquibment
    {
        public int DistrictId { get; set; }
        public District District { get; set; }
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
        public DateTime StartDate { get; set; }
    }
}
