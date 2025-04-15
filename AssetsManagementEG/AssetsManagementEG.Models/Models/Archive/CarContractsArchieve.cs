using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.Models.Models.Archive
{
    public class CarContractsArchieve
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int CarPlateNume { get; set; }
        public int ContractId { get; set; }
        public string ContractName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
