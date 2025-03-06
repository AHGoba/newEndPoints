using AssetsManagementEG.DTOs.CompanyL;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AssetsManagementEG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyLController : ControllerBase
    {
        CompanyLRepository companyLRepository;
        public CompanyLController ( CompanyLRepository _companyLRepository)
        {
            companyLRepository = _companyLRepository;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var companyL = companyLRepository.GetAll();
            var query = companyL.Select(c => new
            {
                CompanyID = c.CompanyID,
                Name = c.Name,
                Location = c.Location
            });

            return Ok(query);
        }


        [HttpPost]
        public IActionResult Create( CreateCompanyLDTOs createCompanyLDTOs ) {

            if (companyLRepository.CompanyLExists(createCompanyLDTOs.Name))
            {
                return BadRequest("Company already exists.");
            }

            var companyL1 = new CompanyL()
            {
                Name = createCompanyLDTOs.Name,
                Location = createCompanyLDTOs.Location
            };

            companyLRepository.Create(companyL1);
            return Ok("Company was successfully added");
        }
        



    }
}
