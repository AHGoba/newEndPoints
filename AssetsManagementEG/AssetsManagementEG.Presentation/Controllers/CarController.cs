﻿using AssetsManagementEG.Context.Context;
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
        ContractCarRepository ContractCarRepository;
        MContractCarsRepo mContractCarsRepo;

        public CarController(DBSContext _context,CarRepository carRepository,
            DistrictRepository _districtrepository , MDistrictCarRepo _mDistrictCarRepo,
            ContractCarRepository _contractCarRepository, MContractCarsRepo _mContractCarsRepo)
        {
            CarRepository = carRepository;
            DistrictRepository = _districtrepository;
            mDistrictCarRepo = _mDistrictCarRepo;
            ContractCarRepository = _contractCarRepository;
            mContractCarsRepo = _mContractCarsRepo;
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

            // check the existing of the car 
            var existingCar = CarRepository.CarExists(c.PlateNum);

            //لو العربية مش موجوده قبل كده 
            if (existingCar == false)
            {
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
                var districtCar = new DistrictCar
                {
                    CarId = car.CarId,
                    DistrictId = district.DistrictId,
                    StartDate = DateTime.Now
                };
                //step 4: Now create Contractcars using carID - contractId - start date 
                // check the existing contract that was sent in the request
                var contract = ContractCarRepository.FindContract(c.contractId);
                if (contract == null)
                {
                    return BadRequest("Failed to create the car.");
                }
                ContractsCars contractsCars = new ContractsCars()
                {
                    CarId = car.CarId,
                    ContractId = c.contractId,
                    StartDate = DateTime.Now
                };

                mDistrictCarRepo.Create(districtCar);
                mContractCarsRepo.Create(contractsCars);
                return Ok($"The car was created successfully and assigned to district {district.Name} and added to the contract {contract.ContractName}");
            }
            else 
            {
                var car = CarRepository.FindCar(c.PlateNum);
                car.IsInService = true;
                CarRepository.Update(car);
                return Ok($"The car with plate number {car.PlateNum} is now in service.");
            }
            //طيب العربية لو موجوده قبل كده ؟ 
        }


        // فى الابديت دا فى 3 حاجات 
        // اول حاجه ان الشخص يغير فى حاجات عاديه زى الاسم وهكذا 
        // تانى حاجه ان هوا لو عايز يغير حالته من فى الخدمه الى خارج الخدمه
        // تالت حاجه انى اغير منطقه العربيه
        [HttpPut]
        [Route("{Id}")]
        public IActionResult Update(CreateOrUpdateCarsDTO c, int Id)
        {
            var existingCar = CarRepository.FindOneForUdpdateOrDelete(Id);
            if (existingCar == null)
            {
                return NotFound("Car not found");
            }

            // Update only the properties that are provided in the request
            if (!string.IsNullOrEmpty(c.Type))
            {
                existingCar.Type = c.Type;
            }

            if (!string.IsNullOrEmpty(c.PlateNum))
            {
                existingCar.PlateNum = c.PlateNum; // Update PlateNum only if it's provided
            }
          
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
                    mDistrictCarRepo.Create(districtCar);

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

            var districtCar = mDistrictCarRepo.FindDistrictCar(Id);
            if (districtCar != null) 
            {
                var archiveRecord = new CarArchive
                {
                    CarId = existingCar.CarId,
                    DistrictId = districtCar.DistrictId,
                    Type = existingCar.Type,
                    PlateNum = existingCar.PlateNum,
                    StartDate = districtCar.StartDate,
                    EndDate = DateTime.Now 
                };
            }
        }
        public IActionResult Delete(int Id)
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
