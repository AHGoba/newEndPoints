using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.DTOs.Tasks
{
    public class CreateTasksDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        //status of task
        public int DistrictId { get; set; }
        public List<int> CarsId { get; set; }
        public List<int> LaborsId { get; set; }
        public List<int> EquibmentsId { get; set; }


    }
}
