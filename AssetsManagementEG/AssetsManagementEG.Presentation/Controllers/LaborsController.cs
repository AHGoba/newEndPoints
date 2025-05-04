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
using AssetsManagementEG.Context.Context;
using AssetsManagementEG.DTOs.Cars;

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
        distLaborArchieveRepo DistLaborArchieveRepo;
        public LaborsController(LaborsRepository laborsRepository, DistrictRepository _districtRepository, 
            MDistrictLaborsRepo _mDistrictLaborsRepo, CompanyLRepository _companyLRepository, MCompanyLaborsRepo _mCompanyLaborsRepo
           , compLaborArchieveRepo _compLaborArchieveRepo
            , distLaborArchieveRepo _DistLaborArchieveRepo)
        {
            LaborsRepository = laborsRepository;
            DistrictRepository = _districtRepository;
            mDistrictLaborsRepo = _mDistrictLaborsRepo;
            companyLRepository = _companyLRepository;
            mCompanyLaborsRepo = _mCompanyLaborsRepo;
            compLaborArchieveRepo = _compLaborArchieveRepo;
            DistLaborArchieveRepo = _DistLaborArchieveRepo;

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

        //[HttpDelete]
        //[Route("{Id}")]
        //public IActionResult Delete(int Id)
        //{
        //    var existingWorker = LaborsRepository.FindOneForUdpdateOrDelete(Id);
        //    if (existingWorker == null)
        //    {
        //        return NotFound("Car not found");
        //    }

        //    LaborsRepository.Delete(existingWorker);
        //    return Ok("The Worker was deleted successfully");
        //}


        [HttpDelete]
        [Route ("ArchiveLabor/{laborId}")]
        public IActionResult ArchiveLabor (int laborId)
        {
            var existingLabor = LaborsRepository.FindOneForUdpdateOrDelete(laborId);
            if (existingLabor == null || !existingLabor.IsAvailable)
            {
                return NotFound("Labor was not found or not available"); 
            }

            var districtLabor = mDistrictLaborsRepo.FindDistrictLabor(laborId);
            if (districtLabor == null)
            {
                return BadRequest("DistrictCar not found, cannot archive the Labor.");
            }



            //get company name related to this labor 
            var companylaborRecord = mCompanyLaborsRepo.FindCompanyLaborsRecordByLaobr(existingLabor.LaborsId);
            if (companylaborRecord == null)
            {
                return BadRequest("CompanyLabor record not found for this labor.");
            }
            var company = companyLRepository.FindOneForUdpdateOrDelete(companylaborRecord.ComapanyID);

            //adding old record to DistLaborArchieve 
            var archiveRecord = new DistLaborArchieve
            {
                LaborId = existingLabor.LaborsId,
                FullName = existingLabor.FullName,
                PhoneNumber = existingLabor.PhoneNumber,
                Position = existingLabor.Position,
                companyName = company.Name,
                DistrictId = districtLabor.DistrictId,
                StartDate = districtLabor.StartDate,
                EndDate = DateTime.Now
            };


            existingLabor.IsInService = false;

            DistLaborArchieveRepo.Create(archiveRecord);
            mDistrictLaborsRepo.Delete(districtLabor);
            LaborsRepository.Update(existingLabor);

            return Ok("The Labor was archived successfully.");
        }


        [HttpPost("ReturnLaborToService")]
        public IActionResult ReturnLaborToService(ChangeLaborStateDTO dto)
        {
            // 1. التحقق من وجود العامل

            var existingLabor = LaborsRepository.FindOneForUdpdateOrDelete(dto.LaborId);
            if (existingLabor == null)
            {
                return NotFound("The specified labor was not found.");
            }

            if (existingLabor.IsInService == true)
            {
                return BadRequest("This labor is already in service.");
            }

            // 2. التحقق من أن الدستركت اللي هيترجع ليه موجود
            var district = DistrictRepository.FindOneForUdpdateOrDelete(dto.DistrictId);
            if (district == null)
            {
                return NotFound("The specified district was not found.");
            }

            // 3. تغيير حالة العامل إلى "في الخدمة"
            existingLabor.IsInService = true;
            LaborsRepository.Update(existingLabor);

            // 4. إضافة سجل جديد للعامل في DistrictLabors
            var newDistrictRecord = new DistrictLabors
            {
                LaborsId = dto.LaborId,
                DistrictId = dto.DistrictId,
                StartDate = DateTime.Now
            };
            mDistrictLaborsRepo.Create(newDistrictRecord);

            // 5. ✅ لو بعت شركة جديدة يتم تحديثها كمان
            if (!string.IsNullOrEmpty(dto.CompanyName))
            {
                var company = companyLRepository.FindCompany(dto.CompanyName);
                if (company == null)
                {
                    return BadRequest($"Company with name {dto.CompanyName} does not exist.");
                }

                var oldCompanyLaborRecord = mCompanyLaborsRepo.FindCompanyLaborsRecordByLaobr(existingLabor.LaborsId);
                if (oldCompanyLaborRecord != null)
                {
                    // أرشفة السجل القديم
                    var compLaborArchive = new CompLaborArchieve()
                    {
                        CompanyId = oldCompanyLaborRecord.ComapanyID,
                        LaborId = existingLabor.LaborsId,
                        FullName = existingLabor.FullName,
                        PhoneNumber = existingLabor.PhoneNumber,
                        Position = existingLabor.Position,
                        CompanyName = company.Name,
                        StartDate = oldCompanyLaborRecord.StartDate,
                        EndDate = DateTime.Now
                    };
                    compLaborArchieveRepo.Create(compLaborArchive);

                    // حذف السجل القديم
                    mCompanyLaborsRepo.Delete(oldCompanyLaborRecord);
                }

                // إضافة سجل جديد للشركة الجديدة
                var newCompanyLabor = new CompanyLabors()
                {
                    ComapanyID = company.CompanyID,
                    LaborsID = existingLabor.LaborsId,
                    StartDate = DateTime.Now
                };
                mCompanyLaborsRepo.Create(newCompanyLabor);
            }

            return Ok($"Labor '{existingLabor.FullName}' has been returned to service and assigned to district '{district.Name}'.");
        }

        ///////////// get labors out of service 
        [HttpGet("OutOfService")]
        public IActionResult GetLaborsOutOfService()
        {
        var labors = LaborsRepository.GetAll()
        .Where(l => l.IsInService == false)
        .Include(l => l.CompanyLabors)
        .ThenInclude(cl => cl.CompanyL)
        .Select(l => new
        {
        laborId = l.LaborsId,
        laborFullName = l.FullName,
        laborPhoneNumber = l.PhoneNumber,
        laborPosition = l.Position,
        CompanyName = l.CompanyLabors
                        .OrderByDescending(cl => cl.StartDate)
                        .Select(cl => cl.CompanyL.Name)
                        .FirstOrDefault()
        })
         .ToList();

            return Ok(labors);
        }





        ////////////////////////// Super User to move from one district to another 
    }
}
