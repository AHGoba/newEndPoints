using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.DTOs.Equipment
{
    public class GetAllEquipmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }

    }
}
