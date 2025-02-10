using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsManagementEG.Models.Models
{
    public class TaskCar
    {
        [ForeignKey("Task")]
        public int TaskId { get; set; } // Foreign key to Task
        public Tassk Task { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; } // Foreign key to Car
        public Car Car { get; set; }
    }
}
