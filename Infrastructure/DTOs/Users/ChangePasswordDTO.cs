namespace Infrastructure.DTOs.Users;

public class ChangePasswordDTO
{
    public string OldPassword { get; set; } = null!;

    public string NewPassword { get; set; } = null!;
}
