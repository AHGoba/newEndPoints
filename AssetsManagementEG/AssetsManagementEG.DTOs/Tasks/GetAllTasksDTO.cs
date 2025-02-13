using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.DTOs.Tasks
{
    public class GetAllTasksDTO
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }

        public List<string> carsNames { get; set; }
        public List<string> equipmentsNames { get; set; }
        public List<string> laborsNames { get; set; }
    }
}
