namespace Profkom.Models;

public class VolunteerQuestionModel
{
    public string Id { get; set; }

    public string VolunteerId { get; set; }
    
    public DateTime Date { get; set; }
    
    public string Question { get; set; }
    public bool IsAnswered { get; set; }
}