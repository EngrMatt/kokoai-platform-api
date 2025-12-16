using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using kokoai_platform_api.DTOs.User;

namespace kokoai_platform_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UserController(
        UserManager<IdentityUser<Guid>> userManager, 
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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
        
        var roles = await _userManager.GetRolesAsync(user);
        
        var userDto = new 
        { 
            user.Id,
            user.UserName, 
            user.Email,
            Roles = roles
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
    
    [HttpPost("role")]
    public async Task<IActionResult> AddUserToRole([FromBody] UserRoleDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // 1. 檢查使用者是否存在
        // 透過 UserId 查找使用者
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            // HTTP 404
            return NotFound($"找不到 ID 為 {model.UserId} 的使用者。");
        }

        // 2. 檢查角色是否存在
        // 透過 RoleName 查找角色
        if (!await _roleManager.RoleExistsAsync(model.RoleName))
        {
            // HTTP 404
            return NotFound($"找不到名稱為 {model.RoleName} 的角色。請先創建角色。");
        }

        // 3. 將使用者加入角色
        // AddToRoleAsync 方法會將使用者與角色關聯起來 (寫入 AspNetUserRoles 表)
        var result = await _userManager.AddToRoleAsync(user, model.RoleName);

        if (result.Succeeded)
        {
            // 成功：返回 204 No Content，表示伺服器已成功處理請求，但無需返回實體。
            return NoContent(); 
        }

        // 4. 處理失敗 (例如使用者已經在該角色中，或資料庫寫入失敗)
        foreach (var error in result.Errors)
        {
            // 將 Identity 錯誤訊息添加到 ModelState
            ModelState.AddModelError(string.Empty, error.Description);
        }
        // HTTP 400 Bad Request
        return BadRequest(ModelState);
    }
}