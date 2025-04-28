using AssetsManagementEG.DTOs.CompanyL;
using AssetsManagementEG.DTOs.ContractCar;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Repositories.Many_ManyRepo;
using AssetsManagementEG.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AssetsManagementEG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractCarController : ControllerBase
    {
        ContractCarRepository ContractCarRepository;
        DistrictRepository DistrictRepository;
        MDistrictCarRepo mDistrictCarRepo;

        public ContractCarController(ContractCarRepository _ContractCarRepository
            , DistrictRepository _districtRepository , MDistrictCarRepo _mDistrictCarRepo)
        {
            ContractCarRepository = _ContractCarRepository;
            DistrictRepository = _districtRepository;
            mDistrictCarRepo = _mDistrictCarRepo;
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
                IsAvailable = c.IsAvailable
                
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
                contractorId = c.CarContractorsId,
                IsAvailable = c.IsAvailable

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
                CarContractorsId = createOrUpdateContractsDTO.carContractorsId,
                IsAvailable = true

            };

            ContractCarRepository.Create(contract);

            return Ok("Contract was successfully added");
        }

        [HttpPost("GetContractsForDistricts")]
        public IActionResult GetContractsForDistricts([FromBody] List<int> districtIds)
        {
            if (districtIds == null || !districtIds.Any())
            {
                return BadRequest("Please provide at least one district ID.");
            }

            var contracts = ContractCarRepository.GetContractsByDistricts(districtIds);

            if (contracts == null || !contracts.Any())
            {
                return NotFound("لم يتم العثور على عقود مرتبطة بالمناطق المحددة.");
            }

            var result = contracts.Select(c => new
            {
                ContractId = c.ContractId,
                ContractName = c.ContractName,
                IsAvailable = c.IsAvailable,
                CarContractorsName = c.CarContractors != null ? c.CarContractors.Name : null
            }).ToList();

            return Ok(result);
        }

    }
}
