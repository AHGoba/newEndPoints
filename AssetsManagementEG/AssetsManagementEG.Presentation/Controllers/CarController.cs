using AssetsManagementEG.Context.Context;
using AssetsManagementEG.DTOs.Cars;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Repositories.Repositories;
using AssetsManagementEG.Repositories.Many_ManyRepo;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using AssetsManagementEG.Repositories.ArchiveRepo;
using System.Diagnostics.Contracts;
using AssetsManagementEG.Models.Models.Archive;

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
        CarArchiveRepo carArchiveRepo;
        CarContractsArchieveRepo carContractsArchieveRepo;

        public CarController(DBSContext _context, CarRepository carRepository,
            DistrictRepository _districtrepository, MDistrictCarRepo _mDistrictCarRepo,
            ContractCarRepository _contractCarRepository, MContractCarsRepo _mContractCarsRepo,
            CarArchiveRepo _carArchiveRepo, CarContractsArchieveRepo _carContractsArchieveRepo)
        {
            CarRepository = carRepository;
            DistrictRepository = _districtrepository;
            mDistrictCarRepo = _mDistrictCarRepo;
            ContractCarRepository = _contractCarRepository;
            mContractCarsRepo = _mContractCarsRepo;
            carArchiveRepo = _carArchiveRepo;
            carContractsArchieveRepo = _carContractsArchieveRepo;
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
            var district = DistrictRepository.FindDistrict(c.DistrictName);
            if (district == null)
            {
                return BadRequest($"The district with name {c.DistrictName} does not exist.");
            }

            var existingCar = CarRepository.CarExists(c.PlateNum);

            // لو العربية مش موجودة
            if (!existingCar)
            {
                //create car 
                var car = new Car()
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
                
                //adding car to district 
                var districtCar = new DistrictCar
                {
                    CarId = car.CarId,
                    DistrictId = district.DistrictId,
                    StartDate = DateTime.Now
                };
                mDistrictCarRepo.Create(districtCar);

                if (!c.IsCompanyOwned)
                {
                    // دا معناه ان العربية دى هتكون ايجار وهضيفها لعقد معين 
                    var contract = ContractCarRepository.FindContract(c.contractId);
                    if (contract == null)
                        return BadRequest($"Contract with ID {c.contractId} does not exist.");

                    contract.IsAvailable = false;
                    ContractCarRepository.Update(contract);

                    var contractsCars = new ContractsCars()
                    {
                        CarId = car.CarId,
                        ContractId = c.contractId,
                        StartDate = DateTime.Now
                    };
                    mContractCarsRepo.Create(contractsCars);

                    return Ok($"The car was created successfully, assigned to district {district.Name}, and added to contract {contract.ContractName}");
                }

                return Ok($"The car was created successfully and assigned to district {district.Name} (company-owned, no contract needed).");
            }
            // لو العربية موجودة بالفعل
            else
            {
                var car = CarRepository.FindCar(c.PlateNum);

                if (car.IsInService == true)
                {
                    return BadRequest("This car is already assigned to a district.");
                }

                car.IsInService = true;
                CarRepository.Update(car);

                var newDistrictCar = new DistrictCar()
                {
                    DistrictId = district.DistrictId,
                    CarId = car.CarId,
                    StartDate = DateTime.Now,
                };
                mDistrictCarRepo.Create(newDistrictCar);

                if (!c.IsCompanyOwned)
                {
                    var assignedContract = ContractCarRepository.FindContract(c.contractId);
                    if (assignedContract == null)
                        return BadRequest($"Contract with ID {c.contractId} does not exist.");

                    assignedContract.IsAvailable = false;
                    ContractCarRepository.Update(assignedContract);

                    var newContractCar = new ContractsCars()
                    {
                        CarId = car.CarId,
                        ContractId = c.contractId,
                        StartDate = DateTime.Now
                    };
                    mContractCarsRepo.Create(newContractCar);

                    return Ok($"The car with plate number {car.PlateNum} is now in service, assigned to district {district.Name}, and added to contract {assignedContract.ContractName}.");
                }

                return Ok($"The car with plate number {car.PlateNum} is now in service and assigned to district {district.Name} (company-owned, no contract needed).");
            }
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

        // change the inservice of the car as if we deleted it and add its history in the archive table
        [HttpPost]
        [Route("ArchiveCar")]
        public IActionResult ArchiveCar(ChangeCarStateDTO cDTo)
        {
            var existingCar = CarRepository.FindOneForUdpdateOrDelete(cDTo.CarId);
            if (existingCar == null || !existingCar.IsAvailable)
            {
                return NotFound("Car was not found or not available: assigned to another task.");
            }

            var districtCar = mDistrictCarRepo.FindDistrictCar(cDTo.CarId);
            if (districtCar == null)
            {
                return BadRequest("DistrictCar not found, cannot archive the car.");
            }

            // Create archive record for the car
            var archiveRecord = new CarArchieve 
            {
                CarId = existingCar.CarId,
                DistrictId = districtCar.DistrictId,
                Type = existingCar.Type,
                PlateNum = existingCar.PlateNum,
                StartDate = districtCar.StartDate,
                EndDate = DateTime.Now,
            };
            carArchiveRepo.Create(archiveRecord);

            // لو بعث ContractId نأرشف العقد القديم
            if ( cDTo.ContractId != 0 || cDTo.isUser == true )  
            {
                var oldContractCar = mContractCarsRepo.FindOneForUpdateOrDelete(cDTo.CarId);
                if (oldContractCar != null)
                {
                       

                    if (districtCar == null || oldContractCar == null)
                    {
                        return BadRequest("Required data not found.");
                    }
                    var archiveContractCar = new CarContractsArchieve
                    {
                        CarId = existingCar.CarId,
                        CarPlateNume = existingCar.PlateNum,
                        ContractId = oldContractCar.ContractId,
                        DistrictName=districtCar.District?.Name,
                        ContractName= oldContractCar.Contract?.ContractName,
                        StartDate = oldContractCar.StartDate,
                        EndDate = DateTime.Now
                        
                    };
                    carContractsArchieveRepo.Create(archiveContractCar);

                    // حذف ربط العربية بالعقد القديم
                    mContractCarsRepo.Delete(oldContractCar);

                    var contract = ContractCarRepository.FindContract(oldContractCar.ContractId);
                    if (contract != null)
                    {
                        contract.IsAvailable = true;
                        ContractCarRepository.Update(contract);
                    }
                }
            }

            // Set car out of service and remove from district
            existingCar.IsInService = false;
            CarRepository.Update(existingCar);
            mDistrictCarRepo.Delete(districtCar);

            return Ok("The car was archived successfully.");
        }




        [HttpPost]
        [Route("ReactivateCar")]
        public IActionResult ReactivateCar(ChangeCarStateDTO cDTo)
        {
            var existingCar = CarRepository.FindOneForUdpdateOrDelete(cDTo.CarId);
            if (existingCar == null)
            {
                return NotFound("Car was not found.");
            }

            var district = DistrictRepository.FindOneForUdpdateOrDelete(cDTo.DistrictId);
            if (district == null)
            {
                return NotFound("District not found.");
            }

            existingCar.IsInService = true;
            CarRepository.Update(existingCar);

            var newDistrictCar = new DistrictCar
            {
                DistrictId = cDTo.DistrictId,
                CarId = existingCar.CarId,
                StartDate = DateTime.Now,
            };
            mDistrictCarRepo.Create(newDistrictCar);

            if ( cDTo.ContractId != 0)
            {
                //// هنا انا عايز اجيب العقد القديم عشان اخلى حالته ب isAvaialble >> True 
                //var oldContract = existingCar.ContractsCars.Where(cc => cc.CarId == existingCar.CarId)
                //    .Select(x => x.Contract.ContractName).FirstOrDefault();

                // لو بعت عقد جديد اربط العربية بيه
                var contract = ContractCarRepository.FindContract(cDTo.ContractId);
                if (contract == null)
                {
                    return BadRequest($"The contract with Id {cDTo.ContractId} does not exist.");
                }

                contract.IsAvailable = false;
                ContractCarRepository.Update(contract);

                var newContractCar = new ContractsCars
                {
                    CarId = existingCar.CarId,
                    ContractId = cDTo.ContractId,
                    StartDate = DateTime.Now,
                };
                mContractCarsRepo.Create(newContractCar);
            }
            // لو ما بعتش حاجة ➔ يفضل مرتبط بالعقد القديم تلقائي بدون تغيير.

            return Ok($"The car with plate number {existingCar.PlateNum} is now reactivated and assigned to tritrict '{district.Name}'.");
        }

        [HttpGet("OutOfService")]
        public IActionResult GetOutOfServiceCars()
        {
            var cars = CarRepository.GetAll()
                .Where(car => car.IsInService == false); // Filter out-of-service cars

            var result = cars.Select(car => new 
            {
                CarId = car.CarId,
                cartype = car.Type,
                carPlateNum = car.PlateNum,
                isAvailable = car.IsAvailable,
                carIsCompanyOwned = car.IsCompanyOwned,
                carIsInService = car.IsInService
            }).ToList();

            return Ok(result);
        }





    }
}

        //[HttpDelete]
        //[Route("{Id}")]
        //public IActionResult Delete(int Id)
        //{
        //    var existingCar = CarRepository.FindOneForUdpdateOrDelete(Id);
        //    if (existingCar == null)
        //    {
        //        return NotFound("Car not found");
        //    }

        //    CarRepository.Delete(existingCar);
        //    return Ok("The car was deleted successfully");
        //}