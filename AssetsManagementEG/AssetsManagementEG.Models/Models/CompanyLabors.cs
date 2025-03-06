using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Models.Models
{
    public class CompanyLabors
    {
        public int ComapanyID { get; set; }
        public CompanyL CompanyL { get; set; }
        public int LaborsID { get; set; }
        public Labors Labors { get; set; }
        public DateTime StartDate { get; set; }
    }
}
