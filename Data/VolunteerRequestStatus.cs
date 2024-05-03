using System.ComponentModel.DataAnnotations;

namespace Profkom.Data;

public class VolunteerRequestStatus
{
    [Key] public string Id { get; set; }

    public string Description { get; set; }
}