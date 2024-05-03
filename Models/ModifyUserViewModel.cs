using Profkom.Data;

namespace Profkom.Models;

public class ModifyUserViewModel
{
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? SelectedRole { get; set; }

    public IList<AppRole>? AvailableRoles { get; set; }
    public IList<string>? Roles { get; set; }
}