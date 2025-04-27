using AssetsManagementEG.DTOs.Cars;
using AssetsManagementEG.DTOs.Equipment;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Models.Models.Archive;
using AssetsManagementEG.Repositories.ArchiveRepo;
using AssetsManagementEG.Repositories.Many_ManyRepo;
using AssetsManagementEG.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AssetsManagementEG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        EquipmentRepository EquipmentRepository;
        DistrictRepository DistrictRepository;
        MDistrictEquipmentRepo mDistrictEquipmentRepo;
        EquipmentArchieveRepo equipmentArchieveRepo;
        public EquipmentController(EquipmentRepository equipmentRepository,
            DistrictRepository _districtRepository, 
            MDistrictEquipmentRepo _mDistrictEquipmentRepo,
            EquipmentArchieveRepo _equipmentArchieveRepo)
        {
            EquipmentRepository = equipmentRepository;
            DistrictRepository = _districtRepository;
            mDistrictEquipmentRepo = _mDistrictEquipmentRepo;
            equipmentArchieveRepo = _equipmentArchieveRepo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var equipment = EquipmentRepository.GetAll();
            var query = equipment.Select(e => new GetAllEquipmentDTO
            {
                Id = e.EquipmentId,
                Name = e.Name,
                Type = e.Type,
                IsAvailable = e.IsAvailable,
            }).ToList();
            return Ok(query);
        }

        [HttpPost]
        public IActionResult Create(CreateOrUpdateEquipmentDTO c)
        {
            // Step 1: Get District
            var district = DistrictRepository.districts()
                .FirstOrDefault(d => d.Name == c.DistrictName);

            if(district == null)
            {
                return BadRequest($"The district with name {c.DistrictName} does not exist.");
            }
            // Step 2: Create equipment then save it 
            //check if exist 

            Equipment equipment = new Equipment()
            { 
                Name = c.Name,
                Type = c.Type,
                IsAvailable = true,
                IsInService=true,
            };

            var result = EquipmentRepository.Create(equipment);
            if (!result)
            {
                return BadRequest("Failed to create the Equipment.");
            }

            // Step 3: Now Create DistrictEquipment Using the Saved CarId and district 
            DistrictEquibment districtEquibment =new DistrictEquibment()
            {
                EquipmentId = equipment.EquipmentId,
                DistrictId = district.DistrictId,
                StartDate = DateTime.Now
            };
            mDistrictEquipmentRepo.Create(districtEquibment);
            return Ok($"The equipment was created successfully and assigned to district {district.Name}");
        }

        [HttpPut]
        [Route("{Id}")]
        public IActionResult Update(CreateOrUpdateEquipmentDTO c, int Id)
        {
            var existingEquipment = EquipmentRepository.FindOneForUdpdateOrDelete(Id);
            if (existingEquipment == null)
            {
                return NotFound("Equipment not found");
            }

            // Update only the properties that are provided in the request
            if (!string.IsNullOrEmpty(c.Name))
            {
                existingEquipment.Name = c.Name;
            }

            if (!string.IsNullOrEmpty(c.Type))
            {
                existingEquipment.Type = c.Type; 
            }
            // هنا لو الشخص دخل قيمه بتفيد ان الشخص فى الخدمه او لاء 
            if (c.IsInService != null)
            {
                // here if he send an value it will assign it 
                // but if he send like space or somthing else it will put it false 
                existingEquipment.IsInService = c.IsInService ?? true;

            }
            // طيب لو هوا بعت منطقه جديدة ساعتها  
            if (c.DistrictName != null)
            {
                if (DistrictRepository.DistrictExists(c.DistrictName))
                {
                    var district = DistrictRepository.districts().FirstOrDefault(d => d.Name == c.DistrictName);
                    DistrictEquibment districtEquibment = new DistrictEquibment()
                    {
                        DistrictId = district.DistrictId,
                        EquipmentId= existingEquipment.EquipmentId,
                        StartDate = DateTime.Now
                    };
                    mDistrictEquipmentRepo.Create(districtEquibment);

                }
                else
                {
                    return BadRequest($"the distrcit {c.DistrictName} doesn't exist");
                }

            }

            EquipmentRepository.Update(existingEquipment);
            return Ok("The equipment was updated successfully");
        }

        [HttpDelete]
        [Route("{Id}")]
        public IActionResult Delete(int Id)
        {
            var existingEquipment = EquipmentRepository.FindOneForUdpdateOrDelete(Id);
            if (existingEquipment == null)
            {
                return NotFound("Car not found");
            }

            EquipmentRepository.Delete(existingEquipment);
            return Ok("The car was deleted successfully");
        }


        // Super User EndPoint

        [HttpPost ("ChangeEquipmentDistrict")]
        public IActionResult ChangeEquipmentDistrict (ChangeEquipmentLocationDTO equipDto)
        {
            var existingEquipment = EquipmentRepository.FindOneForUdpdateOrDelete(equipDto.EquipmentId);
            if (existingEquipment == null || !existingEquipment.IsAvailable)
            {
                return NotFound("Equipment was not found or not available");
            }

            var districtEquipment = mDistrictEquipmentRepo.FindDistrictEquipment(existingEquipment.EquipmentId);
            if (districtEquipment == null)
            {
                return BadRequest("districtEquipment not found, cannot archive the Equipment.");
            }
            var district = DistrictRepository.FindOneForUdpdateOrDelete(equipDto.DistrictId);

            // adding old record to EquipmentArchieve
            var archiveRecord = new EquipmentArchieve
            {
                EquipmentId = existingEquipment.EquipmentId,
                Name = existingEquipment.Name,
                DistrcitId = districtEquipment.DistrictId,
                Type = existingEquipment.Type,
                StartDate = districtEquipment.StartDate,
                EndDate = DateTime.Now,
            };

            equipmentArchieveRepo.Create(archiveRecord);

            //remove old record from districtEquipment as it will be double complex primaryKey
            mDistrictEquipmentRepo.Delete(districtEquipment);

            //adding a new districtEquipment record with new district location
            var newdistrictEquipment = new DistrictEquibment
            {
                DistrictId = equipDto.DistrictId,
                EquipmentId = equipDto.EquipmentId,
                StartDate = DateTime.Now
            };

            mDistrictEquipmentRepo.Create(newdistrictEquipment);



            return Ok($"Equipment '{existingEquipment.Name}' assigned to district '{district.Name}'.");



        }


    }
}
