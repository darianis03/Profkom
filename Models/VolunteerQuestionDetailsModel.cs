namespace Profkom.Models;

public class VolunteerQuestionDetailsModel
{
    public string VolunteerIdentifier { get; set; }
    public string QuestionTitle { get; set; }
    public string Question { get; set; }
    public DateTime QuestionDate { get; set; }
    
    public string Answer { get; set; }
    public DateTime AnswerDate { get; set; }
    public string AnsweredPersonIdentifier { get; set; }
}