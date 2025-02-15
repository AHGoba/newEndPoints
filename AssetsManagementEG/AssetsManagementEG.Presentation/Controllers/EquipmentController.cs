using AssetsManagementEG.DTOs.Cars;
using AssetsManagementEG.DTOs.Equipment;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Repositories.Many_ManyRepo;
using AssetsManagementEG.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public EquipmentController(EquipmentRepository equipmentRepository,
            DistrictRepository _districtRepository, MDistrictEquipmentRepo _mDistrictEquipmentRepo)
        {
            EquipmentRepository = equipmentRepository;
            DistrictRepository = _districtRepository;
            mDistrictEquipmentRepo = _mDistrictEquipmentRepo;
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
                StartDate = c.StartDate
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

            existingEquipment.Name = c.Name;
            existingEquipment.Type = c.Type;
            existingEquipment.IsAvailable = c.IsAvailable;
            existingEquipment.IsInService = c.IsInService;

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
    }
}
