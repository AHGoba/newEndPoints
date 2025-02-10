using AssetsManagementEG.DTOs.Roles;
using AssetsManagementEG.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AssetsManagementEG.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        public RoleManager<IdentityRole> roleManager { get; }
        public UserManager<ApplicationUser> userManager { get; }
        public RoleController( RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleDTO createRoleDTO)
        {
            if (string.IsNullOrWhiteSpace(createRoleDTO.RoleName))
            {
                return BadRequest("Role name is required.");
            }

            // Check if the role already exists
            var roleExists = await roleManager.RoleExistsAsync(createRoleDTO.RoleName);
            if (roleExists)
            {
                return BadRequest("Role already exists.");
            }

            // Create the role
            var result = await roleManager.CreateAsync(new IdentityRole(createRoleDTO.RoleName));
            if (result.Succeeded)
            {
                return Ok($"Role '{createRoleDTO.RoleName}' created successfully.");
            }

            return BadRequest($"Failed to create role. '{result.Errors.ElementAt(0)}'");
        }


        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(AssignRoleDTO assignRoleDTO)
        {
            var user = await userManager.FindByNameAsync(assignRoleDTO.Username);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the role exists
            var roleExists = await roleManager.RoleExistsAsync(assignRoleDTO.RoleName);
            if (!roleExists)
            {
                return BadRequest("Role does not exist.");
            }

            // Assign the role to the user
            var result = await userManager.AddToRoleAsync(user, assignRoleDTO.RoleName);
            if (result.Succeeded)
            {
                return Ok($"Role '{assignRoleDTO.RoleName}' assigned to user '{assignRoleDTO.Username}'.");
            }

            return BadRequest("Failed to assign role.");
        }
    }
}
