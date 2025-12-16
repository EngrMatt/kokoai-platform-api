using System.ComponentModel.DataAnnotations;

namespace kokoai_platform_api.DTOs.User;

public class UserCreateDto
{
    // UserName
    [Required(ErrorMessage = "帳號是必填項。")]
    [StringLength(64, ErrorMessage = "帳號長度不能超過 {1} 個字元。")]
    public required string UserName { get; set; }
    
    // Email
    [Required(ErrorMessage = "電子郵件是必填項。")]
    [EmailAddress(ErrorMessage = "電子郵件格式不正確。")]
    [StringLength(256, ErrorMessage = "電子郵件長度不能超過 {1} 個字元。")]
    public required string Email { get; set; }
    
    // Password
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "密碼必須至少有 {2} 個字元長度。", MinimumLength = 12)]
    public required string Password { get; set; }
    
    // PhoneNumber
    public string? PhoneNumber { get; set; }
}