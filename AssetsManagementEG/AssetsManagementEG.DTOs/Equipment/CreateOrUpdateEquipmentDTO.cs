using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.DTOs.Equipment
{
    public class CreateOrUpdateEquipmentDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }
        public string DistrictName { get; set; }
        public bool? IsInService { get; set; }
        public DateTime StartDate { get; set; }
    }
}
