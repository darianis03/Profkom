using System.ComponentModel.DataAnnotations;

namespace Profkom.Data;

public class VolunteerRequest
{
    [Key] public string Id { get; set; }

    public string VolunteerId { get; set; }
    public Volunteer Volunteer { get; set; }

    public string StatusId { get; set; }
    public VolunteerRequestStatus Status { get; set; }

    public DateTime Date { get; set; }
}