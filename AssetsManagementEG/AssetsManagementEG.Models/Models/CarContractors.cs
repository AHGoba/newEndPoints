using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AssetsManagementEG.Models.Models
{
    public class CarContractors
    {
        // key below related to the P.k
        [Key]
        public int CarContractorsId { get; set; }
        public string Name { get; set; }
        public int phoneNum { get; set; }
        public ICollection<Car> cars { get; set; }
    }
}
