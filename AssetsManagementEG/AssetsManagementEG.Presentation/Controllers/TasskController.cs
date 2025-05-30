﻿using System.Linq;
using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AssetsManagementEG.Repositories.Repositories;
using AssetsManagementEG.DTOs.Districts;
using AssetsManagementEG.DTOs.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

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
        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var Tasks = TaskRepository.GetAll();

        //    List<string> carsList = new List<string>();
        //    List<string> laborsList = new List<string>();
        //    List<string> equipmentsList = new List<string>();



        //    foreach(var task in Tasks)
        //    {
        //        // getting the cars relating to this task 
        //        var carsId = context.TaskCar
        //            .Where(tc => tc.TaskId == task.TaskId)
        //            .Select(tc => tc.CarId).ToList();
        //        var cars = context.Car.Where(c => carsId.Contains(c.CarId)).ToList();
        //        foreach (var car in cars)
        //        {
        //            carsList.Add(car.PlateNum);
        //        }
        //        // getting the Euipments relating to this task 
        //        var equipId = context.TaskEquipment
        //            .Where(te => te.TaskId == task.TaskId)
        //            .Select(te => te.EquipmentId).ToList();
        //        var equipments = context.Equipment.Where(e => equipId.Contains(e.EquipmentId)).ToList();
        //        foreach (var equip in equipments)
        //        {
        //            equipmentsList.Add(equip.Name);
        //        }
        //        // getting the Labors relating to this task 
        //        var laborId = context.TaskLabors
        //            .Where(tl => tl.TaskId == task.TaskId)
        //            .Select(tl => tl.LaborsId).ToList();
        //        var labors = context.Labors.Where(l => laborId.Contains(l.LaborsId)).ToList();
        //        foreach (var labor in labors)
        //        {
        //            laborsList.Add(labor.FullName);
        //        }
        //    }

        //    var query = Tasks.Select(d => new GetAllTasksDTO
        //    {
        //        TaskId = d.TaskId,
        //        Name = d.Name,
        //        Description = d.Description,
        //        IsCompleted = d.IsCompleted,
        //        StartDate = d.StartDate,
        //        EndDate = d.EndDate,
        //        carsNames = carsList,
        //        equipmentsNames = equipmentsList,
        //        laborsNames = laborsList

        //    }).ToList();
        //    return Ok(query);
        //}

        #region GetAll() for Admin
        // this end point is for the (Admin)  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        [HttpGet]
        public IActionResult GetAll()
        {
            var tasks = context.Tassk?.ToList() ?? new List<Tassk>();


            // Load all related data safely 
            // ودا عن طريق ان لو مرجعش حاجه من الداتا بيز فى اى مرحله 
            // اديله حاجه فاضيه 
            //
            var taskCars = context.TaskCar.ToList() ?? new List<TaskCar>();
            var cars = context.Car.ToList() ?? new List<Car>();

            var taskEquipments = context.TaskEquipment.ToList() ?? new List<TaskEquipment>();
            var equipments = context.Equipment.ToList() ?? new List<Equipment>();

            var taskLabors = context.TaskLabors.ToList() ?? new List<TaskLabors>();
            var labors = context.Labors.ToList() ?? new List<Labors>();
            

            var query = tasks.Where(task => task != null).Select(task => new GetAllTasksDTO
            {
                TaskId = task.TaskId,
                DistrictId = task.DistrictId,
                TaskName = task.Name,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                

                // Get cars related to this task
                carsNames = taskCars.Where(tc => tc.TaskId == task.TaskId)
                                    .Join(cars, tc => tc.CarId, c => c.CarId, (tc, c) => c.PlateNum)
                                    .ToList() ?? new List<string>(),

                // Get equipment related to this task
                equipmentsNames = taskEquipments.Where(te => te.TaskId == task.TaskId)
                                                .Join(equipments, te => te.EquipmentId, e => e.EquipmentId, (te, e) => e.Name)
                                                .ToList() ?? new List<string>(),

                // Get labors related to this task
                laborsNames = taskLabors.Where(tl => tl.TaskId == task.TaskId)
                                        .Join(labors, tl => tl.LaborsId, l => l.LaborsId, (tl, l) => l.FullName)
                                        .ToList() ?? new List<string>()

            }).ToList();

            return Ok(query);
        }
        #endregion

        //super user
        #region GetOnGoingDistrictTasks/{Id} EndPoint
        // this end point is for the SuperUsers >>>> get the tasks related to specific (district) with it's (state)
        [HttpPost("GetOnGoingDistrictsTasks")]
        public IActionResult GetOnGoingDistrictsTasks( List<int> districtIds)
        {
            //Get the district 
            var district = context.District.Where(d => districtIds.Contains(d.DistrictId)).ToList();
            if (district == null)
            {
                return NotFound("District not found");
            }

            var tasks = context.Tassk?.Where(t => districtIds.Contains(t.DistrictId))
                .ToList() ?? new List<Tassk>();


            // Load all related data safely                 
            // ودا عن طريق ان لو مرجعش حاجه من الداتا بيز فى اى مرحله 
            // اديله حاجه فاضيه 
            //
            var taskCars = context.TaskCar.ToList();
            var cars = context.Car.ToList();

            var taskEquipments = context.TaskEquipment.ToList();
            var equipments = context.Equipment.ToList();

            var taskLabors = context.TaskLabors.ToList();
            var labors = context.Labors.ToList();


            var query = tasks
                .Where(task => task != null && task.IsCompleted == false)
                .Select(task => new GetAllTasksDTO
            {
                TaskId = task.TaskId,
                TaskName = task.Name,
                DistrictId = task.DistrictId,
                DistrictName = district.FirstOrDefault(d=> d.DistrictId == task.DistrictId).Name,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                StartDate = task.StartDate,
                EndDate = task.EndDate,


                // Get cars related to this task
                carsNames = taskCars.Where(tc => tc.TaskId == task.TaskId)
                                    .Join(cars, tc => tc.CarId, c => c.CarId, (tc, c) => c.PlateNum)
                                    .ToList() ?? new List<string>(),

                // Get equipment related to this task
                equipmentsNames = taskEquipments.Where(te => te.TaskId == task.TaskId)
                                                .Join(equipments, te => te.EquipmentId, e => e.EquipmentId, (te, e) => e.Name)
                                                .ToList() ?? new List<string>(),

                // Get labors related to this task
                laborsNames = taskLabors.Where(tl => tl.TaskId == task.TaskId)
                                        .Join(labors, tl => tl.LaborsId, l => l.LaborsId, (tl, l) => l.FullName)
                                        .ToList() ?? new List<string>()

            }).ToList();

            return Ok(query);
        }
        #endregion
        #region GetEndedDistrictTasks/{Id} EndPoint
        // this end point is for the SuperUsers >>>> get the tasks related to specific (district) with it's (state)
        [HttpPost("GetEndedDistrictsTasks")]
        public IActionResult GetEndedDistrictsTasks(List<int> districtIds)
        {
            //Get the district 
            var district = context.District.Where(d => districtIds.Contains(d.DistrictId)).ToList();
            if (district == null)
            {
                return NotFound("District not found");
            }

            var tasks = context.Tassk?.Where(t => districtIds.Contains(t.DistrictId))
                .ToList() ?? new List<Tassk>();


            // Load all related data safely                 
            // ودا عن طريق ان لو مرجعش حاجه من الداتا بيز فى اى مرحله 
            // اديله حاجه فاضيه 
            //
            var taskCars = context.TaskCar.ToList();
            var cars = context.Car.ToList();

            var taskEquipments = context.TaskEquipment.ToList();
            var equipments = context.Equipment.ToList();

            var taskLabors = context.TaskLabors.ToList();
            var labors = context.Labors.ToList();


            var query = tasks
                .Where(task => task != null && task.IsCompleted == true)
                .Select(task => new GetAllTasksDTO
                {
                    TaskId = task.TaskId,
                    TaskName = task.Name,
                    DistrictId = task.DistrictId,
                    DistrictName = district.FirstOrDefault(d => d.DistrictId == task.DistrictId).Name,
                    Description = task.Description,
                    IsCompleted = task.IsCompleted,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,


                    // Get cars related to this task
                    carsNames = taskCars.Where(tc => tc.TaskId == task.TaskId)
                                    .Join(cars, tc => tc.CarId, c => c.CarId, (tc, c) => c.PlateNum)
                                    .ToList() ?? new List<string>(),

                    // Get equipment related to this task
                    equipmentsNames = taskEquipments.Where(te => te.TaskId == task.TaskId)
                                                .Join(equipments, te => te.EquipmentId, e => e.EquipmentId, (te, e) => e.Name)
                                                .ToList() ?? new List<string>(),

                    // Get labors related to this task
                    laborsNames = taskLabors.Where(tl => tl.TaskId == task.TaskId)
                                        .Join(labors, tl => tl.LaborsId, l => l.LaborsId, (tl, l) => l.FullName)
                                        .ToList() ?? new List<string>()

                }).ToList();

            return Ok(query);
        }
        #endregion

        //user
        #region GetOnGoingTasks/{Id} EndPoint
        // this end point is for the Users >>>>get the tasks with state >>is (ongoing) and related to (districtId)<<<<<<<<<<<<<<
        [HttpGet("GetOnGoingTasks/{Id}")]
        public IActionResult Get(int Id)
        {
            //get the district 
            var district = context.District.FirstOrDefault(d => d.DistrictId == Id);
            if (district == null)
            {
                return NotFound("District not found");
            }

            //get all tasks related to this district 
            var tasks = context.Tassk
                .Where(t => t.DistrictId == district.DistrictId && t.IsCompleted == false)
                .Select( t=> new
                {
                    TaskId = t.TaskId,
                    TaskName = t.Name,
                    Description = t.Description,
                    StartDate = t.StartDate
                })
                .ToList();                     
            return Ok(tasks);
        }
        #endregion

        #region CreateTask EndPoint
        [HttpPost]
        public IActionResult Create(CreateTasksDTO c)
        {
            Tassk task = new Tassk()
            {
                Name = c.Name,
                StartDate = DateTime.Now,
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
                    //first get the car 
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
                    // this if related to if the car is وسيلة مواصلات عاديه 
                    if(car.PlateNum != "1111")
                    {
                        car.IsAvailable = false;
                    }

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

            context.Tassk.Add(task);
            context.SaveChanges();

            return Ok("The Task was created successfully with it's cars,equipments and labors");

        }
        #endregion

        #region UpdatTask EndPoint
        [HttpPut("{Id}")]
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
            existingTask.EndDate = DateTime.Now;
            existingTask.IsCompleted = true;

               //getting car associative with task
            var carIds = context.TaskCar
                .Where(tc => tc.TaskId == existingTask.TaskId)
                .Select(tc => tc.CarId).ToList();
              //change the state of each car to be >> Avillable
            if (carIds != null)
            {
                var carsToUpdate = context.Car.Where(c => carIds.Contains(c.CarId)).ToList();
                foreach(var car in carsToUpdate)
                {
                    car.IsAvailable = true;
                }
            }

              //geting equipment associative with task
            var equipIds = context.TaskEquipment
                .Where(te => te.TaskId == existingTask.TaskId)
                .Select(te => te.EquipmentId).ToList();
              ////change the state of each equipment to be >> Avillable
            if(equipIds != null)
            {
                var equipToUpdate = context.Equipment.Where(e => equipIds.Contains(e.EquipmentId)).ToList();
                foreach(var equip in equipToUpdate)
                {
                    equip.IsAvailable = true;
                }
            }

              //geting labor associative with task
            var laborIds = context.TaskLabors
                .Where(tl => tl.TaskId == existingTask.TaskId)
                .Select(tl => tl.LaborsId).ToList();
              ////change the state of each labor to be >> Avillable
            if(laborIds != null)
            {
                var laborToUpdate = context.Labors.Where(l => laborIds.Contains(l.LaborsId)).ToList();
                foreach(var labor in laborToUpdate)
                {
                    labor.IsAvailable = true;
                }
            }



              // in the update below there are save changes 
            TaskRepository.Update(existingTask);

            return Ok("The Task was updated successfully");

        }
        #endregion



        //Get Tasks for Dashboard ( superUser and Admin ) 

        [HttpPost("GetOldestOnGoingTasksWithDetailsForDistricts")]
        public IActionResult GetOldestOnGoingTasksWithDetailsForDistricts(List<int> districtIds)
        {
            // تحقق إن المناطق موجودة
            var districts = context.District
                .Where(d => districtIds.Contains(d.DistrictId))
                .ToList();

            if (!districts.Any())
            {
                return NotFound("No districts found for the provided IDs.");
            }

            // نجيب كل التاسكات الغير مكتملة
            var tasks = context.Tassk
                .Where(t => districtIds.Contains(t.DistrictId) && t.IsCompleted == false)
                .OrderBy(t => t.StartDate)
                .ToList();

            if (!tasks.Any())
            {
                return Ok("No ongoing tasks found for the provided districts.");
            }

            // نجهز الجداول المساعدة (عشان نربط المهمات بالعمال والعربيات والمعدات)
            var taskCars = context.TaskCar.ToList();
            var cars = context.Car.ToList();
            var taskEquipments = context.TaskEquipment.ToList();
            var equipments = context.Equipment.ToList();
            var taskLabors = context.TaskLabors.ToList();
            var labors = context.Labors.ToList();

            // نرجع أول تاسك لكل دستركت بكل التفاصيل
            var oldestTasksPerDistrict = tasks
                .GroupBy(t => t.DistrictId)
                .Select(g => g.OrderBy(t => t.StartDate).First())
                .Select(task => new
                {
                    TaskId = task.TaskId,
                    TaskName = task.Name,
                    Description = task.Description,
                    DistrictId = task.DistrictId,
                    DistrictName = districts.FirstOrDefault(d => d.DistrictId == task.DistrictId)?.Name,
                    StartDate = task.StartDate,

                    // ✅ نرجع العربيات المرتبطة
                    carsNames = taskCars.Where(tc => tc.TaskId == task.TaskId)
                                   .Join(cars,
                                   tc => tc.CarId,
                                   c => c.CarId,
                                   (tc, c) => c.PlateNum)
                                   .ToList(),

                    // شرح للحوار اللى فوق دا 
                    //var result =
                    //table1
                    //.Where(condition_on_table1) // فلترة مبدئية (اختياري)
                    //.Join(
                    //table2,                  // التابل التاني اللي عايز أربطه
                    //table1KeySelector,        // المفتاح الأساسي من الجدول الأول
                    // table2KeySelector,        // المفتاح الأساسي من الجدول التاني
                    //(t1, t2) => whatYouWant    // النتيجة اللي عايز ترجعها
                    //    )
                    //.ToList();                     // رجّعهم كلهم في List

                    // ✅ نرجع المعدات المرتبطة
                    equipmentsNames = taskEquipments.Where(te => te.TaskId == task.TaskId)
                                               .Join(equipments, te => te.EquipmentId, e => e.EquipmentId, (te, e) => e.Name)
                                               .ToList(),

                    // ✅ نرجع العمال المرتبطين
                    laborsNames = taskLabors.Where(tl => tl.TaskId == task.TaskId)
                                       .Join(labors, tl => tl.LaborsId, l => l.LaborsId, (tl, l) => l.FullName)
                                       .ToList()
                })
                .ToList();

            return Ok(oldestTasksPerDistrict);
        }

    }
}