using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text;

namespace AssetsManagementEG.Models.Models
{
    public class CompanyL
    {
        [Key]
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public ICollection<CompanyLabors> CompanyLabors { get; set; }
    }
}
