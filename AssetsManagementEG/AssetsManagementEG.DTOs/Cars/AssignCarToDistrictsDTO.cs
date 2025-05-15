using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.DTOs.Cars
{
    public class AssignCarToDistrictsDTO
    {
        public int CarId { get; set; }
        public List<int> DistrictIds { get; set; }
    }
}
