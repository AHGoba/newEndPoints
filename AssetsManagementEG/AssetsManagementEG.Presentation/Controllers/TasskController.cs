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
        DBSContext context;
        public TasskController(TaskRepository taskRepository, DBSContext _dBSContext)
        {
            TaskRepository = taskRepository;
            context = _dBSContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var Tasks = TaskRepository.GetAll();
            var query = Tasks.Select(d => new GetAllTasksDTO
            {
                TaskId = d.TaskId,
                Name = d.Name,
                Description = d.Description,
                IsCompleted = d.IsCompleted,
                StartDate = d.StartDate,
                EndDate = d.EndDate
            }).ToList();
            return Ok(query);
        }
        [HttpPost]
        public IActionResult Create(CreateTasksDTO c)
        {
            Tassk task = new Tassk()
            {
                Name = c.Name,
                StartDate = c.StartDate,
                Description = c.Description,
                DistrictId = c.DistrictId,
                IsCompleted= false,
            };

         
            // relate task with car 
            // اول حاجه اجيب العربيه بعدها اتاتكد انها متاحه بعدها بغير حالتها بعدها بربطها بتاسك معين
            if(c.CarsId != null)
            {
                foreach (var id in c.CarsId)
                {
                    //first
                    var car = context.Car.FirstOrDefault(c => c.CarId == id);
                    // or var car = await _context.Cars.FindAsync(carId);
                    //second
                    if(car == null || !car.IsAvailable || !car.IsInService)
                    {
                        return BadRequest("السيارة غير موجوده او مشغوله بخدمه تانيه");
                    }
                    //third 
                    // الطريقة الاولى دى افضل لانه بيربط التاسك اللى اتعمل بال كار ودا اضمن طبعا 
                    task.TaskCars.Add(new TaskCar { CarId = id });
                    //TaskCar taskCar = new TaskCar() { TaskId = Task.TaskId, CarId = car.CarId };
                    //forth
                    car.IsAvailable = false;
                }
            }


            // relate task with equipment
            
            if(c.EquibmentsId != null)
            {
                foreach (var equipId in c.EquibmentsId )
                {                 
                    //first get equip
                    var equipment  = context.Equipment.FirstOrDefault(e=> e.EquipmentId == equipId);
                    //second make a validate
                    if(equipment == null || !equipment.IsAvailable || !equipment.IsInService)
                    {
                        return BadRequest("المعدة غير موجوده او مشغوله بخدمه تانيه");
                    }
                    //third relate equip with task
                    task.TaskEquipment.Add(new TaskEquipment { EquipmentId = equipId });
                    //forth
                    equipment.IsAvailable= false;
                }
            }

            // relate task with labor
            if(c.LaborsId != null)
            {
                foreach (var labId in c.LaborsId)
                {
                    //first get labor
                    var labor = context.Labors.FirstOrDefault(l=> l.LaborsId== labId);
                    //second make a validate
                    if(labor == null || !labor.IsAvailable || !labor.IsInService)
                    {
                        return BadRequest("العامل غير موجود او مشغوله بخدمه تانيه");
                    }
                    //thire relate labor with task
                    task.TaskLabors.Add(new TaskLabors { LaborsId = labId });
                    //forth
                    labor.IsAvailable= false;
                }
            }


            TaskRepository.Create(task);
            context.SaveChanges();

            return Ok("The Task was created successfully with it's cars,equipments and labors");

        }
        [HttpPut]
        [Route("{Id}")]
        public IActionResult Update( UpdateTaskDTO c, int Id)
        {
            var existingTask = TaskRepository.FindOneForUdpdateOrDelete(Id);
            if (existingTask == null)
            {
                return NotFound("Task not found");
            }

            //existingTask.Name = c.Name;
            //existingTask.Description = c.Description;
            //existingTask.StartDate = c.StartDate;
            existingTask.EndDate = c.EndDate;
            existingTask.IsCompleted = true;

            TaskRepository.Update(existingTask);

            return Ok("The Task was updated successfully");

        }
    }
}