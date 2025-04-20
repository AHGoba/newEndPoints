using AssetsManagementEG.DTOs.Equipment;
using AssetsManagementEG.DTOs.Labors;
using AssetsManagementEG.Models.Models;
using AssetsManagementEG.Repositories.Many_ManyRepo;
using AssetsManagementEG.Repositories.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AssetsManagementEG.Repositories.ArchiveRepo;
using AssetsManagementEG.Models.Models.Archive;

namespace AssetsManagementEG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaborsController : ControllerBase
    {
        LaborsRepository LaborsRepository;
        DistrictRepository DistrictRepository;
        MDistrictLaborsRepo mDistrictLaborsRepo;
        CompanyLRepository companyLRepository;
        MCompanyLaborsRepo mCompanyLaborsRepo;
        compLaborArchieveRepo compLaborArchieveRepo;
        public LaborsController(LaborsRepository laborsRepository, DistrictRepository _districtRepository, 
            MDistrictLaborsRepo _mDistrictLaborsRepo, CompanyLRepository _companyLRepository, MCompanyLaborsRepo _mCompanyLaborsRepo
           , compLaborArchieveRepo _compLaborArchieveRepo)
        {
            LaborsRepository = laborsRepository;
            DistrictRepository = _districtRepository;
            mDistrictLaborsRepo = _mDistrictLaborsRepo;
            companyLRepository = _companyLRepository;
            mCompanyLaborsRepo = _mCompanyLaborsRepo;
            compLaborArchieveRepo = _compLaborArchieveRepo;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var Labors = LaborsRepository.GetAll();
            var query = Labors.Include(l => l.CompanyLabors)
            .ThenInclude(cl => cl.CompanyL)
            .Select(e => new GetAllLaborsDTO
            {
                Id = e.LaborsId,
                FullName = e.FullName,
                PhoneNumber = e.PhoneNumber,
                Position = e.Position,
                CompanyName = e.CompanyLabors
                  .OrderByDescending(cl => cl.StartDate) // Sort by latest StartDate
                  .Select(cl => cl.CompanyL.Name)        // Select the company name
                  .FirstOrDefault()                      // Get the latest one in Date
            }).ToList();
            return Ok(query);
        }

        [HttpPost]
        public IActionResult Create(CreateOrUpdateLaborsDTO c)
        {
            // verify the existence of the District in the request
            var district = DistrictRepository.districts()
                .FirstOrDefault(d => d.Name == c.DistrictName);
            if (district == null)
            {
                return BadRequest($"a district with name {c.DistrictName} does not exist.");
            }

            // verify the existence of the company in the request
            var company = companyLRepository.FindCompany(c.CompanyName);
            if (company == null) 
            {
                return BadRequest($"a company with name {c.CompanyName} does not exist");
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
                return BadRequest("Failed to create the worker.");
            }

            // Step 3: Now Create DistrictLabor Using the Saved LaboesId and district 
            DistrictLabors districtLabors = new DistrictLabors()
            {
                DistrictId = district.DistrictId,
                LaborsId = labors.LaborsId,
                StartDate = DateTime.Now
            };

            mDistrictLaborsRepo.Create(districtLabors);

            // Step 3: Now Create CompanyLabor Using the Saved LaboesId and company 
            CompanyLabors companyLabors = new CompanyLabors()
            {
                ComapanyID = company.CompanyID,
                LaborsID = labors.LaborsId,
                StartDate = DateTime.Now
            };

            mCompanyLaborsRepo.Create(companyLabors);
            return Ok($"The Worker {labors.FullName} was created successfully and assigned to district {district.Name} and assigned to the company {company.Name}");
        }


        [HttpPut]
        [Route("{Id}")]
        public IActionResult Update(CreateOrUpdateLaborsDTO c, int Id)
        {
            var existingWorker = LaborsRepository.FindOneForUdpdateOrDelete(Id);
            if (existingWorker == null)
            {
                return NotFound("Employee was not found");
            }

            // Update only the properties that are provided in the request
            if (!string.IsNullOrEmpty(c.FullName) && existingWorker.FullName != c.FullName)
            {
                existingWorker.FullName = c.FullName;
            }

            if (!string.IsNullOrEmpty(c.PhoneNumber) && existingWorker.PhoneNumber != c.PhoneNumber)
            {
                existingWorker.PhoneNumber = c.PhoneNumber; 
            }

            if (!string.IsNullOrEmpty(c.Position) && existingWorker.Position != c.Position)
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
            //// طيب لو هوا بعت منطقه جديدة ساعتها  
            //if ( c.DistrictName != null)
            //{
            //    if (DistrictRepository.DistrictExists(c.DistrictName))
            //    {
            //        var district = DistrictRepository.districts().FirstOrDefault(d=> d.Name == c.DistrictName);
            //        DistrictLabors districtLabors = new DistrictLabors()
            //        {
            //            DistrictId = district.DistrictId,
            //            LaborsId = existingWorker.LaborsId,
            //            StartDate = DateTime.Now
            //        };
            //        mDistrictLaborsRepo.Create( districtLabors );
            //    }
            //    else
            //    {
            //        return BadRequest($"a district with the name {c.DistrictName} does not exist");
            //    }
            //}

            if (c.CompanyName != null)
            {

                // getting company and companyLabor Recored 
                var company = companyLRepository.FindCompany(c.CompanyName);
                var oldcompanyLaborsRecord = mCompanyLaborsRepo.FindCompanyLaborsRecord(company.CompanyID);
                if (company != null && oldcompanyLaborsRecord != null)
                {
                    // adding record to archieve 
                    var complaborarchieveRecord = new CompLaborArchieve()
                    {
                        CompanyId = company.CompanyID,
                        CompanyName = company.Name,
                        LaborId = existingWorker.LaborsId,
                        FullName = existingWorker.FullName,
                        PhoneNumber = existingWorker.PhoneNumber,
                        Position = existingWorker.Position,
                        StartDate = oldcompanyLaborsRecord.StartDate,
                        EndDate = DateTime.Now
                    };


                    CompanyLabors newcompanyLabors = new CompanyLabors()
                    {
                        LaborsID = existingWorker.LaborsId,
                        ComapanyID = company.CompanyID,
                        StartDate = DateTime.Now
                    };

                    compLaborArchieveRepo.Create(complaborarchieveRecord);
                    mCompanyLaborsRepo.Delete(oldcompanyLaborsRecord);
                    mCompanyLaborsRepo.Create(newcompanyLabors);
                }
                else 
                {
                  return BadRequest($"a company with the name {c.CompanyName} does not exist");
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




        ////////////////////////// Super User to move from one district to another 
    }
}
