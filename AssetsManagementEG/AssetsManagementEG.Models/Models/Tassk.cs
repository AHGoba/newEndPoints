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
        public string Comment { get; set; }
        public virtual ICollection<TaskCar> TaskCars { get; set; } = new List<TaskCar>();
        public virtual ICollection<TaskEquipment> TaskEquipment { get; set; } =new List<TaskEquipment>();
        public virtual ICollection<TaskLabors> TaskLabors { get; set; } = new List<TaskLabors>();
        [ForeignKey("District")]
        public int DistrictId { get; set; }
        public virtual District District { get; set; }

    }
}
