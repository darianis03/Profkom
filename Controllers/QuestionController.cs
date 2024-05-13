using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profkom.Data;
using Profkom.Models;
using Profkom.Services;

namespace Profkom.Controllers;

public class QuestionController(
    VolunteerService volunteerService,
    VolunteerQuestionService volunteerQuestionService,
    VolunteerAnswerService volunteerAnswerService,
    UserManager<AppUser> userManager
    ) : Controller
{
    public IActionResult QuestionForm()
    {
        var model = new VolunteerQuestionFormModel
        {
            Question = "",
            QuestionTitle = ""
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> QuestionForm(VolunteerQuestionFormModel model)
    {
        var user = await userManager.GetUserAsync(User);
        var volunteer = await volunteerService.GetByUserIdAsync(user.Id);

        if (volunteer == null) return NotFound();

        var question = new VolunteerQuestion
        {
            Id = Guid.NewGuid().ToString(),
            Date = DateTime.UtcNow,
            QuestionTitle = model.QuestionTitle,
            Question = model.Question,
            VolunteerId = volunteer.Id
        };

        await volunteerQuestionService.AddAsync(question);
        await volunteerQuestionService.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> QuestionDetails(string questionId)
    {
        var question = await volunteerQuestionService.GetByIdAsync(questionId);

        var volunteer = await volunteerService.GetByIdAsync(question?.VolunteerId);
        if (volunteer == null) return NotFound();

        var answer = await volunteerAnswerService.GetByQuestionIdAsync(question.Id);

        var answeredPerson = await userManager.FindByIdAsync(answer?.UserId);

        var model = new VolunteerQuestionDetailsModel
        {
            VolunteerIdentifier = $"{volunteer.FirstName} {volunteer.LastName}" ,
            QuestionTitle = question.QuestionTitle,
            Question = question.Question,
            QuestionDate = question.Date,
            AnsweredPersonIdentifier = answeredPerson?.NormalizedEmail,
            Answer = answer?.Answer,
            AnswerDate = answer.Date
        };

        return View(model);
    }

    public async Task<IActionResult> Questions()
    {
        var questions = await volunteerQuestionService.GetAllAsync();

        if (User.IsInRole("Member") || User.IsInRole("Volunteer"))
        {
            var user = await userManager.GetUserAsync(User);
            var volunteer = await volunteerService.GetByUserIdAsync(user.Id);

            questions = questions.Where(q => q.VolunteerId == volunteer?.Id);
        }

        var model = new VolunteerQuestionsModel
        {
            Questions = new List<VolunteerQuestionModel>()
        };

        foreach (var question in questions)
        {
            var isAnswered = await volunteerAnswerService.IsQuestionAnswered(question.Id);

            model.Questions.Add(new VolunteerQuestionModel
            {
                Id = question.Id,
                VolunteerId = question.VolunteerId,
                Date = question.Date,
                Question = question.Question,
                IsAnswered = isAnswered
            });
        }

        return View(model);
    }

    public async Task<IActionResult> QuestionAnswerForm(string questionId)
    {
        var question = await volunteerQuestionService.GetByIdAsync(questionId);

        var model = new VolunteerAnswerModel
        {
            QuestionId = question?.Id,
            Answer = "",
            Question = question?.Question!,
            QuestionTitle = question?.QuestionTitle!
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> QuestionAnswerForm(VolunteerAnswerModel model)
    {
        var user = await userManager.GetUserAsync(User);

        if (user == null) return NotFound();

        var answer = new VolunteerAnswer
        {
            Id = Guid.NewGuid().ToString(),
            UserId = user.Id,
            Date = DateTime.UtcNow,
            QuestionId = model.QuestionId,
            Answer = model.Answer
        };

        await volunteerAnswerService.AddAsync(answer);
        await volunteerAnswerService.SaveChangesAsync();

        return RedirectToAction("Questions");
    }
}
