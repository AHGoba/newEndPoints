using AssetsManagementEG.DTOs.carContractors;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AssetsManagementEG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarContractorsController : ControllerBase
    {
        CarContractorsRepository CarContractorsRepository;

        public CarContractorsController(CarContractorsRepository _carContractorsRepository)
        {
            CarContractorsRepository = _carContractorsRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var contractors = CarContractorsRepository.GetAll();
            var query = contractors.Select(c => new GetAllContractorsDTO
            {
                CarContractorsId = c.CarContractorsId,
                Name = c.Name,
                phoneNum = c.phoneNum
            }).ToList();
            return Ok(query);
        }


        [HttpPost]
        public IActionResult Create(CreateCarContractorsDTO createCarContractorsDTO)
        {
            try
            {
                // Check if contractor already exists
                var existingContractor = CarContractorsRepository.CheckContractorsExcisting(createCarContractorsDTO.Name);
                if (existingContractor)
                {
                    return BadRequest($"Contractor with name {createCarContractorsDTO.Name} already exists.");
                }

                var carContractors = new CarContractors()
                {
                    Name = createCarContractorsDTO.Name,
                    phoneNum = createCarContractorsDTO.phoneNum
                };

                var result = CarContractorsRepository.Create(carContractors);
                if (!result)
                {
                    return BadRequest("Failed to create the contractor.");
                }

                return Ok("Contractor was added successfully.");
            }
            catch (Exception ex)
            {
                // Log the error if needed
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}
