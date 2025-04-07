using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Models.Models
{
    public class ContractsCars
    {
        public int? ContractId {  get; set; }
        public Contract Contract { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public DateTime StartDate { get; set; }
    }
}
