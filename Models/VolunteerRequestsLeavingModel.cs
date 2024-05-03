using Profkom.Data;

namespace Profkom.Models;

public class VolunteerRequestsLeavingModel
{
    
    public IEnumerable<VolunteerRequestLeaving> Requests { get; set; }
    public IEnumerable<VolunteerRequestStatus> RequestStatuses { get; set; }
}