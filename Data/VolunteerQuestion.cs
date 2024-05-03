using System.ComponentModel.DataAnnotations;

namespace Profkom.Data;

public class VolunteerQuestion
{
    [Key] public string Id { get; set; }

    public string VolunteerId { get; set; }
    public Volunteer Volunteer { get; set; }
    
    public DateTime Date { get; set; }
    
    public string QuestionTitle { get; set; }
    public string Question { get; set; }
}