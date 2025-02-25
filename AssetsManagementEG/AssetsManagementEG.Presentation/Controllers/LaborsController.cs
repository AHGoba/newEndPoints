using AssetsManagementEG.DTOs.Equipment;
using AssetsManagementEG.DTOs.Labors;
using AssetsManagementEG.Models.Models;
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
    public class LaborsController : ControllerBase
    {
        LaborsRepository LaborsRepository;
        DistrictRepository DistrictRepository;
        MDistrictLaborsRepo mDistrictLaborsRepo;
        public LaborsController(LaborsRepository laborsRepository, 
            DistrictRepository _districtRepository, MDistrictLaborsRepo _mDistrictLaborsRepo)
        {
            LaborsRepository = laborsRepository;
            DistrictRepository = _districtRepository;
            mDistrictLaborsRepo = _mDistrictLaborsRepo;

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var Labors = LaborsRepository.GetAll();
            var query = Labors.Select(e => new GetAllLaborsDTO
            {
                Id = e.LaborsId,
                FullName = e.FullName,
                PhoneNumber = e.PhoneNumber,
                Position = e.Position,
            }).ToList();
            return Ok(query);
        }

        [HttpPost]
        public IActionResult Create(CreateOrUpdateLaborsDTO c)
        {
            // Step 1: Get District
            var district = DistrictRepository.districts()
                .FirstOrDefault(d => d.Name == c.DistrictName);
            if (district == null)
            {
                return BadRequest($"The district with name {c.DistrictName} does not exist.");
            }

            // Step 2: Create labor then save it
            Labors labors = new Labors()
            {
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Position = c.Position,
                IsAvailable=true,
                IsInService = true,
            };
            var result = LaborsRepository.Create(labors);
            if (!result)
            {
                return BadRequest("Failed to create the labor.");
            }

            // Step 3: Now Create DistrictLabor Using the Saved LaboesId and district 
            DistrictLabors districtLabors = new DistrictLabors()
            {
                DistrictId = district.DistrictId,
                LaborsId = labors.LaborsId,
                StartDate = DateTime.Now
            };

            mDistrictLaborsRepo.Create(districtLabors);
            return Ok($"The Worker was created successfully and assigned to district {district.Name}");
        }

        [HttpPut]
        [Route("{Id}")]
        public IActionResult Update(CreateOrUpdateLaborsDTO c, int Id)
        {
            var existingWorker = LaborsRepository.FindOneForUdpdateOrDelete(Id);
            if (existingWorker == null)
            {
                return NotFound("Equipment not found");
            }


            // Update only the properties that are provided in the request
            if (!string.IsNullOrEmpty(c.FullName))
            {
                existingWorker.FullName = c.FullName;
            }

            if (!string.IsNullOrEmpty(c.PhoneNumber))
            {
                existingWorker.PhoneNumber = c.PhoneNumber; 
            }

            if (!string.IsNullOrEmpty(c.Position))
            {
                existingWorker.Position = c.Position;
            }


            // هنا لو الشخص دخل قيمه بتفيد ان الشخص فى الخدمه او لاء 
            if (c.IsInService != null)
            {
                // here if he send an value it will assign it 
                // but if he send like space or somthing else it will put it false 
                existingWorker.IsInService = c.IsInService??true ;

            }
            // طيب لو هوا بعت منطقه جديدة ساعتها  
            if ( c.DistrictName != null)
            {
                if (DistrictRepository.DistrictExists(c.DistrictName))
                {
                    var district = DistrictRepository.districts().FirstOrDefault(d=> d.Name == c.DistrictName);
                    DistrictLabors districtLabors = new DistrictLabors()
                    {
                        DistrictId = district.DistrictId,
                        LaborsId = existingWorker.LaborsId,
                        StartDate = DateTime.Now
                    };

                }
                
            }


            LaborsRepository.Update(existingWorker);
            return Ok("The Worker was updated successfully");
        }

        [HttpDelete]
        [Route("{Id}")]
        public IActionResult Delete(int Id)
        {
            var existingWorker = LaborsRepository.FindOneForUdpdateOrDelete(Id);
            if (existingWorker == null)
            {
                return NotFound("Car not found");
            }

            LaborsRepository.Delete(existingWorker);
            return Ok("The Worker was deleted successfully");
        }
    }
}
