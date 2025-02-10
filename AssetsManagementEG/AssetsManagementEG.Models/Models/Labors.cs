using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsManagementEG.Models.Models
{
    public class Labors
    {
        public int LaborsId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public bool IsInService { get; set; }
        public bool IsAvailable { get; set; }
        public virtual ICollection<TaskLabors> TaskLabors { get; set; }
        public virtual ICollection<DistrictLabors> DistrictLabors { get; set; }
    }
}
