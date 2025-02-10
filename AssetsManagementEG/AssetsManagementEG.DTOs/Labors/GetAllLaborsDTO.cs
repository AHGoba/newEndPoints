using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.DTOs.Labors
{
    public class GetAllLaborsDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
    }
}
