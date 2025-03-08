using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.DTOs.Labors
{
    public class CreateOrUpdateLaborsDTO
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public string? DistrictName { get; set; }
        public string? CompanyName { get; set; }
        public bool IsAvailable { get; set; }
        public bool? IsInService { get; set; }
        public DateTime StartDate { get; set; }
    }
}
