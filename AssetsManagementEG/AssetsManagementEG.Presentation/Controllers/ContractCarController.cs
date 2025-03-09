using AssetsManagementEG.DTOs.CompanyL;
using AssetsManagementEG.DTOs.ContractCar;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AssetsManagementEG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractCarController : ControllerBase
    {
        ContractCarRepository ContractCarRepository;
        DistrictRepository DistrictRepository;

        public ContractCarController(ContractCarRepository _ContractCarRepository, DistrictRepository _districtRepository)
        {
            ContractCarRepository = _ContractCarRepository;
            DistrictRepository = _districtRepository;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var contracts = ContractCarRepository.GetAll();
            var query = contracts.Select(c => new
            {
                contractId = c.ContractId,
                contractName = c.ContractName,
                contractType = c.ContractType,
                contractDescribtion = c.ContractDescreption,
                startDate = c.StartDate,
                endDate = c.EndDate,
            });

            return Ok(query);
        }


        [HttpGet("GetDistrictContracts/{Id}")]
        public IActionResult GetDistrictContracts(int Id)
        {
            var district = DistrictRepository.FindOneForUdpdateOrDelete(Id);
            if (district == null)
            {
                return NotFound("District not found");
            }

            var districtContracts = ContractCarRepository.FindContracts(Id).Select(c => new
            {
                contractId = c.ContractId,
                contractName = c.ContractName,
                contractType = c.ContractType,
                contractDescribtion = c.ContractDescreption,
                startDate = c.StartDate,
                endDate = c.EndDate,
            });

            return Ok(districtContracts);

        }

        [HttpPost]

        public IActionResult Create(CreateOrUpdateContractsDTO createOrUpdateContractsDTO)
        {

            if (ContractCarRepository.ContractCarExists(createOrUpdateContractsDTO.ContractName))
            {
                return BadRequest("Contract already exists.");
            }

            var contract = new Contract()
            {
                ContractName = createOrUpdateContractsDTO.ContractName,
                ContractType = createOrUpdateContractsDTO.ContractType,
                ContractDescreption = createOrUpdateContractsDTO.ContractDescreption,
                StartDate = createOrUpdateContractsDTO.StartDate,
                EndDate = createOrUpdateContractsDTO.EndDate,
           
            };

            ContractCarRepository.Create(contract);
            return Ok("Company was successfully added");
        }


    }
}
