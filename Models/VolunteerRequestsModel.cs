using Profkom.Data;

namespace Profkom.Models;

public class VolunteerRequestsModel
{
    
    public IEnumerable<VolunteerRequest> Requests { get; set; }
    public IEnumerable<VolunteerRequestStatus> RequestStatuses { get; set; }
}