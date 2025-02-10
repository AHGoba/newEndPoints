using System.Linq;
using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AssetsManagementEG.Repositories.Repositories;
using AssetsManagementEG.DTOs.Districts;
using AssetsManagementEG.DTOs.Tasks;

namespace AssetsManagementEG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasskController : ControllerBase
    {
        TaskRepository TaskRepository;
        public TasskController(TaskRepository taskRepository)
        {
            TaskRepository = taskRepository;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var Tasks = TaskRepository.GetAll();
            var query = Tasks.Select(d => new GetAllTasksDTO
            {
                Name = d.Name,
                Description = d.Description,
                IsCompleted = d.IsCompleted,
                StartDate = d.StartDate,
                EndDate = d.EndDate
            }).ToList();
            return Ok(query);
        }
        [HttpPost]
        public IActionResult Create(CreateOrUpdateTasksDTO c)
        {
            Tassk Task = new Tassk()
            {
                Name = c.Name,
                Description = c.Description,
                StartDate = c.StartDate,
                IsCompleted= false,

            };
            TaskRepository.Create(Task);
            return Ok("The Task was created successfully");

        }
        [HttpPut]
        [Route("{Id}")]
        public IActionResult Update(CreateOrUpdateTasksDTO c, int Id)
        {
            var existingTask = TaskRepository.FindOneForUdpdateOrDelete(Id);
            if (existingTask == null)
            {
                return NotFound("Task not found");
            }

            existingTask.Name = c.Name;
            existingTask.Description = c.Description;
            existingTask.StartDate = c.StartDate;
            existingTask.EndDate = c.EndDate;
            existingTask.IsCompleted = c.IsCompleted;

            TaskRepository.Update(existingTask);

            return Ok("The Task was updated successfully");

        }
    }
}