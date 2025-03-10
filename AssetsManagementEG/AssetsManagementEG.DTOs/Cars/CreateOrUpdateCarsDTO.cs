using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.DTOs.Cars
{
    public class CreateOrUpdateCarsDTO
    { 
        public string Type { get; set; }
        public string PlateNum { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsCompanyOwned { get; set; }
        public string DistrictName { get; set; }
        public bool? IsInService { get; set; }
        public DateTime StartDate { get; set; }

        public int contractId { get; set; }


    }
}
