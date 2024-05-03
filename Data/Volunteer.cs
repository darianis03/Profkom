using System.ComponentModel.DataAnnotations.Schema;

namespace Profkom.Data;

public class Volunteer
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [InverseProperty("Volunteer")] public virtual ICollection<VolunteerRequest> VolunteerRequests { get; set; }
}