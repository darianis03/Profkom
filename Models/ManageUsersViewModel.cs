using Profkom.Data;

namespace Profkom.Models;

public class ManageUsersViewModel
{
    public List<AppUser>? Users { get; set; }
    public Dictionary<string, List<string>>? UserRoles { get; set; }
}