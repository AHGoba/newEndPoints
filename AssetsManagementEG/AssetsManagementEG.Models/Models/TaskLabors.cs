using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsManagementEG.Models.Models
{
    public class TaskLabors
    {
        public int TaskId { get; set; }
        public Tassk Task { get; set; }
        public int LaborsId { get; set; }
        public Labors Labors { get; set; }
    }
}
