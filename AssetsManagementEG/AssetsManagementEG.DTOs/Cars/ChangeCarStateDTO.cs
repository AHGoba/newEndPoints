using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.DTOs.Cars
{
    public class ChangeCarStateDTO
    {
        public int CarId        { get; set; }
        public int DistrictId   { get; set; }   
        public int ContractId   { get; set; }
        //public bool IsInService { get; set; }
        public bool isUser { get; set; }
    }
}
