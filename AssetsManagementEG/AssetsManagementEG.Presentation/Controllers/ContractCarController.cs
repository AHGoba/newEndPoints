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

            // here in the logic below  i'm sending the id of the district so i can get all contracts related to this id 

            var districtContracts = ContractCarRepository.FindContracts(Id).Select(c => new
            {
                contractId = c.ContractId,
                contractName = c.ContractName,
                contractType = c.ContractType,
                contractDescribtion = c.ContractDescreption,
                startDate = c.StartDate,
                endDate = c.EndDate,
                districtId = c.DistrictId,
                contractorId = c.CarContractorsId
            });

            return Ok(districtContracts);

        }

        [HttpPost]

        public IActionResult Create(CreateOrUpdateContractsDTO createOrUpdateContractsDTO)
        {
            var district = DistrictRepository.FindOneForUdpdateOrDelete(createOrUpdateContractsDTO.districtId);
            if (district == null)
            {
                return NotFound("District not found");
            }

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
                DistrictId= createOrUpdateContractsDTO.districtId,
                CarContractorsId = createOrUpdateContractsDTO.carContractorsId

            };

            ContractCarRepository.Create(contract);

            return Ok("Contract was successfully added");
        }


    }
}
