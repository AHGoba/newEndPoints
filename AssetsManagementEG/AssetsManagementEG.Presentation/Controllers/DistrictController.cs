﻿using Microsoft.AspNetCore.Http;
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
using AssetsManagementEG.Context.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AssetsManagementEG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        DistrictRepository DistrictRepository;
        public DBSContext context;
        public DistrictController(DistrictRepository districtRepository, DBSContext _dBSContext)
        {
            DistrictRepository = districtRepository;
            context = _dBSContext;
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

        /////////////////////////////// get district (labors- equipments - cars) available To (Create Task)
        ///
        //
        //

        [HttpGet("GetDistrictCars/{Id}")]
        public IActionResult GetDistrictCar( int Id)
        {
            var district = DistrictRepository.FindOneForUdpdateOrDelete(Id) ;
            if (district == null)
            {
                return NotFound("District not found");
            }


            //get all carrecords realted to this district from districtcar table
            // in shape of carrecords[101,105,107]

            var carrecords = context.DistrictCar
                .Where(dc => dc.DistrictId == Id )
                .Select(dc=> dc.CarId).ToList() ;

            if (!carrecords.Any())
            {
                return NotFound($"There is no cars assigned to the districtName {district.Name}");
            }
            // رجعلى العربيات اللى تابعه ل id  
            //وكمان تكون متاحه وكمان فى الخدمه!!
            var cars = context.Car
                .Where(c => carrecords.Contains(c.CarId) && c.IsAvailable && c.IsInService)
                .Select(cr => new
                {
                    cartype = cr.Type,
                    carPlateNum = cr.PlateNum,
                    carId = cr.CarId,

                    contractorName = cr.ContractsCars
                    .Select(cc => cc.Contract.CarContractors.Name)
                    .FirstOrDefault()

                    //contractorName = cr.ContractsCars
                    //.Select(cc => context.Contract
                    //.Where(c => c.ContractId  == cc.ContractId)
                    ////.Select(d => d.CarContractorsId)
                    //.Select(ccon=> context.CarContractors
                    //.Where(cC=> cC.CarContractorsId ==ccon.CarContractorsId ))).FirstOrDefault()

                    // عايزين نرجع اسم المقاول 
                    //districtName = cr.DistrictCar.Where(dc => dc.CarId == cr.CarId).Select(d => d.DistrictId).FirstOrDefault()
                }).ToList() ;


            return Ok(cars) ;          
        }

        [HttpGet("GetDistrictEquipments/{Id}")]
        public IActionResult GetDistrictEquipment(int Id)
        {
            // getting district 
            var district = DistrictRepository.FindOneForUdpdateOrDelete(Id);
            if(district == null)
            {
                return NotFound("District not found");
            }

            // get the EquipmentRecords from DisrtictEquipment table 
            var EquipmentRecords = context.DistrictEquibment
                .Where(de => de.DistrictId == district.DistrictId)
                .Select(e => e.EquipmentId).ToList() ;

            if (!EquipmentRecords.Any()) 
            {
                return NotFound($"There is no Equipment assigned to the districtName {district.Name}");
            }

            //getting the equipment from Equipment table

            var Equipments = context.Equipment
                .Where(e => EquipmentRecords.Contains(e.EquipmentId) && e.IsAvailable && e.IsInService)
                .Select(e => new
                {
                    EquipmentName = e.Name,
                    EquipmentType = e.Type,
                    EquipmentId = e.EquipmentId
                });

            return Ok(Equipments) ;
        }

        [HttpGet("GetDistrictLabors/{Id}")]
        public IActionResult GetDistrictLabor(int Id)
        {
            // getting the district
            var district = DistrictRepository.FindOneForUdpdateOrDelete (Id);
            if (district == null)
            {
                return NotFound("District not found");
            }

            // getting LaborRecords from DistrictLabor table 
            var LaborRecords = context.DistrictLabors
                .Where(dl=> dl.DistrictId == district.DistrictId )
                .Select(l => l.LaborsId).ToList() ;

            if (!LaborRecords.Any())
            {
                return NotFound($"There is no Labor assigned to the districtName {district.Name}");
            }

            // getting Labors from labor table 

            var Labors = context.Labors
                .Where(l => LaborRecords.Contains(l.LaborsId) && l.IsAvailable&&l.IsInService)
                .Select(l =>new
                {
                    LaborName = l.FullName,
                    LaborPostion = l.Position,
                    LaborId = l.LaborsId
                });

            return Ok(Labors) ;
        }


        /////////////////////////////// get district (labors- equipments - cars) To Edit District (labors- equipments - cars)
        ///
        [HttpGet("GetDistrictCarsForEdit/{Id}")]
        public IActionResult AONGetDistrictCar(int Id)
        {
            var district = DistrictRepository.FindOneForUdpdateOrDelete(Id);
            if (district == null)
            {
                return NotFound("District not found");
            }


            //get all carrecords realted to this district from districtcar table
            // in shape of carrecords[101,105,107]

            var carrecords = context.DistrictCar
                .Where(dc => dc.DistrictId == Id)
                .Select(dc => dc.CarId).ToList();

            if (!carrecords.Any())
            {
                return NotFound($"There is no cars assigned to the districtName {district.Name}");
            }
            // رجعلى العربيات اللى تابعه ل id  
            //وكمان تكون متاحه وكمان فى الخدمه!!
            var cars = context.Car
                .Where(c => carrecords.Contains(c.CarId) && c.IsInService)
                .Select(cr => new
                {
                    cartype = cr.Type,
                    carPlateNum = cr.PlateNum,
                    carId = cr.CarId,
                    carIsCompanyOwned = cr.IsCompanyOwned,
                    carIsInService = cr.IsInService,
                    contractName = cr.ContractsCars.Select(cc => cc.Contract.ContractName).FirstOrDefault()
                }).ToList();


            return Ok(cars);
        }

        [HttpGet("GetDistrictEquipmentsForEdit/{Id}")]
        public IActionResult AONDistrictEquipment(int Id)
        {
            // getting district 
            var district = DistrictRepository.FindOneForUdpdateOrDelete(Id);
            if (district == null)
            {
                return NotFound("District not found");
            }

            // get the EquipmentRecords from DisrtictEquipment table 
            var EquipmentRecords = context.DistrictEquibment
                .Where(de => de.DistrictId == district.DistrictId)
                .Select(e => e.EquipmentId).ToList();

            if (!EquipmentRecords.Any())
            {
                return NotFound($"There is no Equipment assigned to the districtName {district.Name}");
            }

            //getting the equipment from Equipment table

            var Equipments = context.Equipment
                .Where(e => EquipmentRecords.Contains(e.EquipmentId) && e.IsAvailable && e.IsInService)
                .Select(e => new
                {
                    EquipmentIid = e.EquipmentId,
                    EquipmentName = e.Name,
                    EquipmentType = e.Type,
                    EquipmentIsInService= e.IsInService
                });

            return Ok(Equipments);
        }

        [HttpGet("GetDistrictLaborsForEdit/{Id}")]
        public IActionResult AONDistrictLabor(int Id)
        {
            // getting the district
            var district = DistrictRepository.FindOneForUdpdateOrDelete(Id);
            if (district == null)
            {
                return NotFound("District not found");
            }

            // getting LaborRecords from DistrictLabor table 
            var LaborRecords = context.DistrictLabors
                .Where(dl => dl.DistrictId == district.DistrictId)
                .Select(l => l.LaborsId).ToList();

            if (!LaborRecords.Any())
            {
                return NotFound($"There is no Labor assigned to the districtName {district.Name}");
            }

            // getting Labors from labor table 
            var Labors = context.Labors
        .Where(l => LaborRecords.Contains(l.LaborsId) && l.IsAvailable && l.IsInService)
        .Include(l => l.CompanyLabors)  // Include CompanyLabors
        .ThenInclude(cl => cl.CompanyL)  // Include Company
        .Select(l => new
        {
            LaborId = l.LaborsId,
            LaborFullName = l.FullName,
            LaborPosition = l.Position,
            LaborIsInservice = l.IsInService,
            LaborPhoneNumber = l.PhoneNumber,
            companyName = l.CompanyLabors
                .OrderByDescending(cl => cl.StartDate) // Get latest company
                .Select(cl => cl.CompanyL.Name)
                .FirstOrDefault()
        });

            return Ok(Labors);
        }


        [HttpPost("GetDistrictCarsForSuperUser")]
        public IActionResult GetDistrictCarsForSuperUser([FromBody] List<int> districtIds)
        {
            if (districtIds == null || !districtIds.Any())
            {
                return BadRequest("يرجى تحديد رقم منطقة واحدة على الأقل.");
            }

            // تحقق من أن كل DistrictId موجود فعلاً في قاعدة البيانات
            var existingDistricts = context.District
                .Where(d => districtIds.Contains(d.DistrictId))
                .Select(d => d.DistrictId)
                .ToList();

            var notFoundIds = districtIds.Except(existingDistricts).ToList();

            if (notFoundIds.Any())
            {
                return NotFound($"بعض المناطق غير موجودة: {string.Join(", ", notFoundIds)}");
            }

            // Get all car IDs linked to these districts
            var carIds = context.DistrictCar
                .Where(dc => districtIds.Contains(dc.DistrictId))
                .Select(dc => dc.CarId)
                .Distinct()
                .ToList();

            if (!carIds.Any())
            {
                return NotFound("لا توجد سيارات مرتبطة بهذه المناطق.");
            }

            // استرجاع بيانات السيارات
            var cars = context.Car
                .Where(c => carIds.Contains(c.CarId))
                .Select(cr => new
                {
                    carId = cr.CarId,
                    carPlatenum = cr.PlateNum,
                    Type = cr.Type,
                    IsAvailable = cr.IsAvailable,
                    IsCompanyOwned = cr.IsCompanyOwned,
                    IsInService = cr.IsInService,
                    contractName = cr.ContractsCars
                        .Select(cc => cc.Contract.ContractName)
                        .FirstOrDefault(),
                    contractorName = cr.ContractsCars
                        .Select(cc => cc.Contract.CarContractors.Name)
                        .FirstOrDefault(),
                    districtName = cr.DistrictCar
                        .Select(dc => dc.District.Name)
                        .FirstOrDefault()
                }).ToList();

            return Ok(cars);
        }


        



    }
}
