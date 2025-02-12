using AssetsManagementEG.Context.Context;
using AssetsManagementEG.DTOs.Users;
using AssetsManagementEG.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
        public DBSContext context;
        public UserManager<ApplicationUser> userManager { get; }
		public IConfiguration configuration { get; }

		public AccountController(DBSContext _context, UserManager<ApplicationUser> _userManager, IConfiguration _configuration)
		{
            context = _context;
            userManager = _userManager;
			configuration = _configuration;
		}

		[Route("Register")]
		[HttpPost]
		public async Task<IActionResult> Register(RegisterAndLoginDTO registerAndLoginDTO)
		{
            
			var ExistingUserReg = await userManager.FindByNameAsync(registerAndLoginDTO.Username);
			if (ExistingUserReg != null)
			{
				return Ok("A user with the same Username already exists");
			}
			ApplicationUser user = new ApplicationUser()
			{
				UserName = registerAndLoginDTO.Username
			};
			var result = await userManager.CreateAsync(user, registerAndLoginDTO.Password);

			if (!result.Succeeded)
			{
				return Ok(result.Errors.ElementAt(0));

			}
			return Ok("Created Successfully");
		}

        [HttpPost("Login")]

        public async Task<IActionResult> Login(RegisterAndLoginDTO registerAndLoginDTO)

        {

            var ExistingUserLog = await userManager.FindByNameAsync(registerAndLoginDTO.Username);

            if (ExistingUserLog == null)

            {
                return Unauthorized("you need to register first");
            }

            var CkeckPass = await userManager.CheckPasswordAsync(ExistingUserLog, registerAndLoginDTO.Password);

            if (CkeckPass == false)

            {
                return Unauthorized("The username or password is incorrect");
            }

            var roles = await userManager.GetRolesAsync(ExistingUserLog);

            var userRole = roles.FirstOrDefault() ?? "User";

            var Claims = new List<Claim>()
            {
            new Claim (ClaimTypes.Name, ExistingUserLog.UserName)
            };

            var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));

            var SetToken = new JwtSecurityToken(

            expires: DateTime.Now.AddHours(2),

            claims: Claims,

            signingCredentials: new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256) );

            
            var Token = new JwtSecurityTokenHandler().WriteToken(SetToken);
            //getting district id 
            UsersDistrict _UserDistrict = context.UsersDistrict.FirstOrDefault(ud => ud.ApplicationUserId == ExistingUserLog.ID);

            if (_UserDistrict == null)
            {
                return BadRequest("this user has no district (not assigned)");
            }

            
            
            return Ok(new
            {
                token = Token,
                role = userRole,
                Username = ExistingUserLog.UserName,
                DistrictID = _UserDistrict.DistrictId,
            });
        }

        [HttpPost("AssignDistricts")]
        public async Task<IActionResult> AssignDistrictToUser(AssignDistrictsToUsersDTO assignDistrictsToUsersDTO)
        {
            
            //getting the user
            var user = await userManager.Users
                .Include(u => u.UsersDistricts)
                .FirstOrDefaultAsync(u => u.UserName == assignDistrictsToUsersDTO.Username);
            if (user == null)
            {
                return NotFound("User not found");
            }

            //getting the role of the user
            var role = await userManager.GetRolesAsync(user);

            //Here below if the role of user >> Admin
            //assign all district to him
            if (role.Contains("Admin"))
            {
                var allDistricts = await context.District.ToListAsync();
                user.UsersDistricts = allDistricts.Select(d => new UsersDistrict
                {
                    ApplicationUserId = user.Id,
                    DistrictId = d.DistrictId
                }).ToList();

                await context.SaveChangesAsync();
                return Ok($"All districts have been assigned to the admin '{user.UserName}'.");
            }
            else
            {
                //if this user role is (User or superUser) then assign only one district 
                var district = await context.District
                    .FirstOrDefaultAsync(d => d.Name == assignDistrictsToUsersDTO.DistrictName);

                if (district == null)
                {
                    return NotFound("District not found.");
                }

                var userDistrict = new UsersDistrict
                {
                    ApplicationUserId = user.Id,
                    DistrictId = district.DistrictId
                };

                user.UsersDistricts.Add(userDistrict);

                await context.SaveChangesAsync();
                return Ok($"District '{district.Name}' has been assigned to '{user.UserName}'.");
            }
        }
    }
}
