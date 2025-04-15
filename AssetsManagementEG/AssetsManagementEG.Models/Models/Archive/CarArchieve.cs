using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Models.Models
{
    public class CarArchieve
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int DistrictId { get; set; }
        public string Type { get; set; }
        public string PlateNum { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
