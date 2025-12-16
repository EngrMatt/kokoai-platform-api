using System;

namespace kokoai_platform_api.DTOs.Role;

public class RoleDto
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = null!;
}