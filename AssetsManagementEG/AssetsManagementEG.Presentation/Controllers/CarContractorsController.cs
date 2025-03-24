using AssetsManagementEG.DTOs.carContractors;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Create (CreateCarContractorsDTO createCarContractorsDTO)
        {
            // check the existing of the car 
            var existingContractores = CarContractorsRepository.CheckContractorsExcisting(createCarContractorsDTO.Name);
            if (existingContractores == true) {
                return BadRequest($"The contractors with name {createCarContractorsDTO.Name} does not exist.");
            }

            CarContractors carContractors = new CarContractors()
            {
                Name = createCarContractorsDTO.Name,
                phoneNum = createCarContractorsDTO.phoneNum,
            };

            var result = CarContractorsRepository.Create(carContractors);
            if (!result)
            {
                return BadRequest("Failed to create the car.");
            }

            return Ok("Contractors was added");
        }
    }
}
