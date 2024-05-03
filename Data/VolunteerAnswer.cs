using System.ComponentModel.DataAnnotations;

namespace Profkom.Data;

public class VolunteerAnswer
{
    [Key] public string Id { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }
    
    public string QuestionId { get; set; }
    public VolunteerQuestion Question { get; set; }
    
    public DateTime Date { get; set; }
    
    public string Answer { get; set; }
}