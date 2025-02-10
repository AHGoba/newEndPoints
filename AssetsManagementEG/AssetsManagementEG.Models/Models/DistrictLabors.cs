using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsManagementEG.Models.Models
{
    public class DistrictLabors
    {
        public int DistrictId { get; set; }
        public District District { get; set; }  
        public int LaborsId { get; set; }
        public Labors Labors { get; set; }
        public DateTime StartDate { get; set; }
    }
}
