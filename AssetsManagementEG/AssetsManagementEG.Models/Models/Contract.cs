﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AssetsManagementEG.Models.Models
{
    public class Contract
    {
        public int ContractId { get; set; }
        public string ContractName { get; set; }
        public string ContractType { get; set; }
        public string ContractDescreption { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsAvailable { get; set; }

        [ForeignKey("District")]
        public int DistrictId { get; set; }
        public virtual District District { get; set; }

        //ربط العربية بالعقود
        public virtual ICollection<ContractsCars> ContractsCars { get; set; }

        // ربط العقود بالمقاولين
        // ان كل مقاول مربوط بأكتر من عقد
        [ForeignKey("CarContractors")]
        public int? CarContractorsId { get; set; }
        public virtual CarContractors CarContractors { get; set; }

    }
}
