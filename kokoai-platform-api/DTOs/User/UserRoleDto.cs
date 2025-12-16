using System.ComponentModel.DataAnnotations;

namespace kokoai_platform_api.DTOs.User;

public class UserRoleDto
{
    // UserID
    [Required(ErrorMessage = "使用者 ID 是必填項。")]
    public required string UserId { get; set; } 
    
    // 角色名稱
    [Required(ErrorMessage = "角色名稱是必填項。")]
    public required string RoleName { get; set; }
}