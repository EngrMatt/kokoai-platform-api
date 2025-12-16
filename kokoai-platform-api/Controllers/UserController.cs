using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using kokoai_platform_api.DTOs.User;

namespace kokoai_platform_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;

    public UserController(UserManager<IdentityUser<Guid>> userManager)
    {
        _userManager = userManager;
    }
    
    // 讀取單一使用者
    [HttpGet("GetUserInfoById/{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id); 
        
        if (user == null)
        {
            return NotFound();
        }
        
        var userDto = new 
        { 
            user.Id,
            user.UserName, 
            user.Email 
        };
        return Ok(userDto);
    }
    
    // 建立使用者
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        if (await _userManager.FindByEmailAsync(model.Email) != null)
        {
            ModelState.AddModelError(nameof(model.Email), "電子郵件已被註冊。");
            return BadRequest(ModelState);
        }
        if (await _userManager.FindByNameAsync(model.UserName) != null)
        {
            ModelState.AddModelError(nameof(model.UserName), "帳號名稱已被使用。");
            return BadRequest(ModelState);
        }
        
        var user = new IdentityUser<Guid> 
        { 
            UserName = model.UserName, 
            Email = model.Email, 
            PhoneNumber = model.PhoneNumber,
            EmailConfirmed = false
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            var createdUser = new 
            { 
                user.Id,
                user.UserName, 
                user.Email 
            };
            
            return CreatedAtAction(nameof(GetUser), new { id = user.Id.ToString() }, createdUser);
        }
        
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
        return BadRequest(ModelState);
    }
}