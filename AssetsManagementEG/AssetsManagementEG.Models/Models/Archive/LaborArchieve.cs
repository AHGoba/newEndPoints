using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Models.Models.Archive
{
    public class LaborArchieve
    {
        public int Id { get; set; }
        public int LaborId { get; set; }
        public int DistrictId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
