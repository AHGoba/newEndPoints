using AssetsManagementEG.DTOs.CompanyL;
using AssetsManagementEG.DTOs.Users;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Repositories.Many_ManyRepo;
using AssetsManagementEG.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AssetsManagementEG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyLController : ControllerBase
    {
        CompanyLRepository companyLRepository;
        LaborsRepository laborsRepository;
        MCompanyLaborsRepo mCompanyLaborsRepo;
        public CompanyLController ( CompanyLRepository _companyLRepository, LaborsRepository _laborsRepository, MCompanyLaborsRepo _mCompanyLaborsRepo)
        {
            companyLRepository = _companyLRepository;
            laborsRepository = _laborsRepository;
            mCompanyLaborsRepo = _mCompanyLaborsRepo;
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

        [HttpPost("AssignCompany")]
        public async Task<IActionResult> AssignCompanyToLabors(AssignCompanyToLaborsDTO assignCompanyToLaborsDTO)
        {

            //getting the Labors 
            var ExisitingLabor = laborsRepository.LaborsExists(assignCompanyToLaborsDTO.LaborsFullName);
            if (!ExisitingLabor)
            {
                return NotFound("Labors not found");
            }
            //getting the Company 
            var ExisitingCompany = companyLRepository.CompanyLExists(assignCompanyToLaborsDTO.CompanyName);
            if (!ExisitingCompany)
            {
                return NotFound("Company not found");
            }

            else
            {
                //get the company Info
                var company = companyLRepository.FindCompany(assignCompanyToLaborsDTO.CompanyName);

                if (company == null)
                {
                    return NotFound("Company not found.");
                }
                //get the Labor Info
                var labors = laborsRepository.FindLabors(assignCompanyToLaborsDTO.LaborsFullName);

                if (labors == null)
                {
                    return NotFound("Employee not found.");
                }

                var CompanyLabors = new CompanyLabors
                {
                    LaborsID = labors.LaborsId,
                    ComapanyID = company.CompanyID
                };

                mCompanyLaborsRepo.Create(CompanyLabors);
                return Ok($"Company '{company.Name}' has been assigned to '{labors.FullName}'.");
            }
        }


    }
}
