using AssetsManagementEG.Context.Context;
using AssetsManagementEG.DTOs.Cars;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Repositories.Repositories;
using AssetsManagementEG.Repositories.Many_ManyRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AssetsManagementEG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        CarRepository CarRepository;
        DistrictRepository DistrictRepository;
        MDistrictCarRepo mDistrictCarRepo;

        public CarController(DBSContext _context,CarRepository carRepository,
            DistrictRepository _districtrepository , MDistrictCarRepo _mDistrictCarRepo)
        {
            CarRepository = carRepository;
            DistrictRepository = _districtrepository;
            mDistrictCarRepo = _mDistrictCarRepo;
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var Cars = CarRepository.GetAll();
            var query = Cars.Select(car => new GetAllCarsDTO
            {
                CarId = car.CarId,
                Type = car.Type,
                PlateNum = car.PlateNum,
                IsAvailable = car.IsAvailable,
                IsCompanyOwned = car.IsCompanyOwned,
                IsInService = car.IsInService,
                
            }).ToList();
            return Ok(query);
        }

        [HttpPost]
        public IActionResult Create(CreateOrUpdateCarsDTO c)
        {
            // Step 1: Get District
            var district = DistrictRepository.districts()
                .FirstOrDefault(d => d.Name == c.DistrictName);

            if (district == null)
            {
                return BadRequest($"The district with name {c.DistrictName} does not exist.");
            }
            // Step 2: Create car then save it
            Car car = new Car()
            {
                Type = c.Type,
                PlateNum = c.PlateNum,
                IsAvailable = true,
                IsCompanyOwned = c.IsCompanyOwned,
                IsInService = true,
            };

            var result = CarRepository.Create(car);
            if (!result)
            {
                return BadRequest("Failed to create the car.");
            }

            // relate the car with it's district 
            // Step 3: Now Create DistrictCar Using the Saved CarId and district 
            DistrictCar districtCar = new DistrictCar()
            {
                CarId = car.CarId,  // ✅ Now CarId is valid
                DistrictId = district.DistrictId,
                StartDate = DateTime.Now
            };

            mDistrictCarRepo.Create(districtCar);
            return Ok($"The car was created successfully and assigned to district {district.Name}");
        }


        [HttpPut]
        [Route("{Id}")]
        public IActionResult Update(CreateOrUpdateCarsDTO c, int Id)
        {
            var existingCar = CarRepository.FindOneForUdpdateOrDelete(Id);
            if (existingCar == null)
            {
                return NotFound("Car not found");
            }

            existingCar.Type = c.Type;
            existingCar.PlateNum = c.PlateNum;
            existingCar.IsAvailable = c.IsAvailable;
            existingCar.IsCompanyOwned = c.IsCompanyOwned;
           

            // هنا لو الشخص دخل قيمه بتفيد ان الشخص فى الخدمه او لاء 
            if (c.IsInService != null)
            {
                // here if he send an value it will assign it 
                // but if he send like space or somthing else it will put it false 
                existingCar.IsInService = c.IsInService ?? true;

            }
            // طيب لو هوا بعت منطقه جديدة ساعتها  
            if (c.DistrictName != null)
            {
                if (DistrictRepository.DistrictExists(c.DistrictName))
                {
                    var district = DistrictRepository.districts().FirstOrDefault(d => d.Name == c.DistrictName);
                    DistrictCar districtCar = new DistrictCar()
                    {
                        DistrictId = district.DistrictId,
                        CarId = existingCar.CarId,
                        StartDate = DateTime.Now
                    };

                }

            }

            CarRepository.Update(existingCar);

            return Ok("The car was updated successfully");
        }

        [HttpDelete]
        [Route("{Id}")]
        public IActionResult ChangeServiceState(int Id)
        {
            var existingCar = CarRepository.FindOneForUdpdateOrDelete(Id);
            if (existingCar == null)
            {
                return NotFound("Car not found");
            }

            CarRepository.Delete(existingCar);
            return Ok("The car was deleted successfully");
        }
    }
}
