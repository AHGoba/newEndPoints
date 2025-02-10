using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AssetsManagementEG.Repositories.Repositories;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.DTOs.Districts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AssetsManagementEG.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Routing;
using System.Linq;

namespace AssetsManagementEG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        DistrictRepository DistrictRepository;
        public DistrictController(DistrictRepository districtRepository)
        {
            DistrictRepository = districtRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var Districts = DistrictRepository.GetAll();
            var query = Districts.Select(d => new GetAllDistrictsDTO
            {
                DistrictId = d.DistrictId,
                Name = d.Name,
            }).ToList();
            return Ok(query);
        }

        [HttpPost]
        public IActionResult Create(CreateOrUpdateDistrictDTO c)
        {
            if (DistrictRepository.DistrictExists(c.Name))
            {
                return BadRequest("District already exists.");
            }

            var district = new District
            {
                Name = c.Name
            };
            DistrictRepository.Create(district);
            return Ok("District was successfully added");
        }

        [HttpPut]
        [Route("{Id}")]
        public IActionResult Update(CreateOrUpdateDistrictDTO c, int Id)
        {
            var existingDistrict = DistrictRepository.FindOneForUdpdateOrDelete(Id);
            if (existingDistrict == null)
            {
                return NotFound("District not found");
            }

            existingDistrict.Name = c.Name;
            DistrictRepository.Update(existingDistrict);

            return Ok("The car was updated successfully");
        }
    }
}
