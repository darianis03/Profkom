namespace Profkom.Models;

public class VolunteerRequestDetailsModel
{
    public string Id { get; set; } 
    
    public string VolunteerId { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Status { get; set; }

    public DateTime Date { get; set; } 
}