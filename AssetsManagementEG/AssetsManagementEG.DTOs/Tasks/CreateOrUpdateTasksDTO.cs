using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsManagementEG.DTOs.Tasks
{
    public class CreateOrUpdateTasksDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
