using System.ComponentModel.DataAnnotations;

namespace kokoai_platform_api.DTOs.Role;

public class RoleCreateDto
{
    [Required(ErrorMessage = "角色名稱是必填項。")]
    [StringLength(256, ErrorMessage = "角色名稱長度不能超過 {1} 個字元。")]
    public required string RoleName { get; init; }
}