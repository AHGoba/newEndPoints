using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Models.Models.Archive
{
    public class EquipmentArchieve
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public int DistrcitId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
