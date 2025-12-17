// Controllers/RoleController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using kokoai_platform_api.DTOs.Role;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kokoai_platform_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public RoleController(RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
    }
    
    [HttpPost("CreateRole")]
    public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _roleManager.RoleExistsAsync(model.RoleName))
        {
            ModelState.AddModelError(nameof(model.RoleName), "此角色名稱已存在。");
            return BadRequest(ModelState);
        }
        
        var role = new IdentityRole<Guid>(model.RoleName);

        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            var createdRole = new RoleDto 
            { 
                Id = role.Id, 
                RoleName = role.Name! 
            };
            
            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id.ToString() }, createdRole);
        }
        
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return BadRequest(ModelState);
    }
    
    [HttpGet("GetRoles")]
    public IActionResult GetRoles()
    {
        var roles = _roleManager.Roles
            .Select(r => new RoleDto 
            {
                Id = r.Id,
                RoleName = r.Name!
            })
            .ToList();

        return Ok(roles);
    }

    [HttpGet("GetRoleById/{id}")]
    public async Task<IActionResult> GetRoleById(string id)
    {
        var role = await _roleManager.FindByIdAsync(id); 
        
        if (role == null)
        {
            return NotFound();
        }
        
        var roleDto = new RoleDto 
        { 
            Id = role.Id, 
            RoleName = role.Name!
        };
        return Ok(roleDto);
    }
    
    // ... 可以根據需要新增 PUT (更新) 和 DELETE (刪除) ...
}