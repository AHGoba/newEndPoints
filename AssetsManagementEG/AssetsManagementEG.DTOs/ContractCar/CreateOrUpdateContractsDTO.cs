using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.DTOs.ContractCar
{
    public class CreateOrUpdateContractsDTO
    {
        public string ContractName { get; set; }
        public string ContractType { get; set; }
        public string ContractDescreption { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int districtId { get; set; }
        public int carContractorsId { get; set; }
    }
}
