using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace AssetsManagementEG.Models.Models
{
    public class Tassk
    {
        [Key]
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public virtual ICollection<TaskCar> TaskCars { get; set; }
        public virtual ICollection<TaskEquipment> TaskEquipment { get; set; }
        public virtual ICollection<TaskLabors> TaskLabors { get; set; }
        [ForeignKey("District")]
        public int DistrictId { get; set; }
        public virtual District District { get; set; }

    }
}
