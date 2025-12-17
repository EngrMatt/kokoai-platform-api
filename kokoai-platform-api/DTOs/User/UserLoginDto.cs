using System.ComponentModel.DataAnnotations;

namespace kokoai_platform_api.DTOs.User;

public class UserLoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}