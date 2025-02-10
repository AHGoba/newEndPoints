using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.DTOs.Cars
{
    public class GetAllCarsDTO
    {
        public int CarId { get; set; }
        public string Type { get; set; }
        public string PlateNum { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsCompanyOwned { get; set; }
        public bool IsInService { get; set; }
    }
}
