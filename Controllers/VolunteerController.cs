using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profkom.Data;
using Profkom.Models;
using Profkom.Services;

namespace Profkom.Controllers;

public class VolunteerController(
    VolunteerService volunteerService,
    VolunteerRequestService volunteerRequestService,
    VolunteerRequestStatusService volunteerRequestStatusService,
    VolunteerRequestLeavingService volunteerRequestLeavingService,
    VolunteerQuestionService volunteerQuestionService,
    VolunteerAnswerService volunteerAnswerService,
    UserManager<AppUser> userManager
) : Controller
{
     [Authorize]
    public async Task<IActionResult> Apply()
    {
        var usrStatus = new VolunteerRegistrationModel
        {
            FirstName = "",
            LastName = "",
            RegistrationSuccessful = false
        };
        
        if (User.IsInRole("ExVolunteer") || User.IsInRole("Member")) return View(usrStatus);

        if (User.IsInRole("Volunteer") || User.IsInRole("Admin") || User.IsInRole("Moderator"))
        {
            usrStatus.RegistrationSuccessful = true;
            return View(usrStatus);
        }
        
        return View(usrStatus);
    }

    [HttpPost]
    public async Task<IActionResult> Apply(VolunteerRegistrationModel model)
    {
        if (ModelState.IsValid)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var usrId = currentUser?.Id;
            
            if (currentUser != null)
            {
                var volunteer = new Data.Volunteer
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = usrId,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                await volunteerService.AddAsync(volunteer);
                await volunteerService.SaveChangesAsync();
                var status = await volunteerRequestStatusService.GetByDescriptionAsync("Pending");
                var volunteerRequest = new VolunteerRequest
                {
                    Id = Guid.NewGuid().ToString(),
                    VolunteerId = volunteer.Id,
                    StatusId = status.Id,
                    Date = DateTime.UtcNow
                };

                await volunteerRequestService.AddAsync(volunteerRequest);
                await volunteerRequestService.SaveChangesAsync();
            }

            var usrStatus = new VolunteerRegistrationModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                RegistrationSuccessful = true
            };

            return View(usrStatus);
        }

        var usrStandard = new VolunteerRegistrationModel
        {
            FirstName = "",
            LastName = "",
            RegistrationSuccessful = false
        };

        return View(usrStandard);
    }
    
    [Authorize]
    public async Task<IActionResult> Requests()
    {
        var firstStatus = new VolunteerRequestStatus
        {
            Id = Guid.NewGuid().ToString(),
            Description = "Pending"
        };
        await volunteerRequestStatusService.AddAsync(firstStatus);
        
        var secondStatus = new VolunteerRequestStatus
        {
            Id = Guid.NewGuid().ToString(),
            Description = "Accepted"
        };
        await volunteerRequestStatusService.AddAsync(secondStatus);
        
        var thirdStatus = new VolunteerRequestStatus
        {
            Id = Guid.NewGuid().ToString(),
            Description = "Rejected"
        };
        await volunteerRequestStatusService.AddAsync(thirdStatus);
        
        var requests = await volunteerRequestService.GetAllAsync();
        var requestStatuses = await volunteerRequestStatusService.GetAllAsync();
        
        var model = new VolunteerRequestsModel
        {
            Requests = requests,
            RequestStatuses = requestStatuses
        };
        
        return View("Requests", model);
    }
    
    [Authorize]
    public async Task<IActionResult> AcceptRequest(string requestId)
    {
        var currentRequest = await volunteerRequestService.GetByIdAsync(requestId);
        if (currentRequest == null) return NotFound();
        
        var currentVolunteer = await volunteerService.GetByIdAsync(currentRequest.VolunteerId);
        if (currentVolunteer == null) return NotFound();
        
        var currentUser = await userManager.FindByIdAsync(currentVolunteer.UserId);
        if (currentUser != null)
        {
            var newStatus = await volunteerRequestStatusService.GetByDescriptionAsync("Accepted");
            
            if ( newStatus == null ) return NotFound();
            
            currentRequest.StatusId = newStatus.Id;
            await volunteerRequestStatusService.SaveChangesAsync();
            await volunteerRequestService.UpdateAsync(currentRequest);
            await userManager.RemoveFromRoleAsync(currentUser, "Member");
            await userManager.AddToRoleAsync(currentUser, "Volunteer");
        }

        return RedirectToAction("Requests");
    }
    
    [Authorize]
    public async Task<IActionResult> RejectRequest(string requestId)
    {
        var currentRequest = await volunteerRequestService.GetByIdAsync(requestId);
        if (currentRequest == null) return NotFound();
        
        var currentVolunteer = await volunteerService.GetByIdAsync(currentRequest.VolunteerId);
        if (currentVolunteer == null) return NotFound();
        
        var currentUser = await userManager.FindByIdAsync(currentVolunteer.UserId);
        if (currentUser != null)
        {
            var newStatus = await volunteerRequestStatusService.GetByDescriptionAsync("Rejected");
            
            if ( newStatus != null ) currentRequest.StatusId = newStatus.Id;

            await volunteerRequestService.UpdateAsync(currentRequest);
            await volunteerRequestService.SaveChangesAsync();
        }

        return RedirectToAction("Requests");
    }
    
    [HttpPost]
    public async Task<IActionResult> RequestDetails(string requestId)
    {
        var request = await volunteerRequestService.GetByIdAsync(requestId);
        var volunteer = await volunteerService.GetByIdAsync(request.VolunteerId);
        
        var model = new VolunteerRequestDetailsModel
        {
            Id = request.Id,
            VolunteerId = request.VolunteerId,
            FirstName = volunteer.FirstName,
            LastName = volunteer.LastName,
            Date = request.Date
        };
        
        return View(model);
    }
    
    public IActionResult Leave()
    {
        var model = new VolunteerLeaveModel
        {
            Reason = "",
            Confirm = "",
            IsApplied = false
        };
        
        if (User.IsInRole("Volunteer") || User.IsInRole("Admin") || User.IsInRole("Moderator")) return View(model);

        if (User.IsInRole("ExVolunteer") || User.IsInRole("Member"))
        {
            model.IsApplied = true;
            return View(model);
        }
        
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Leave(VolunteerLeaveModel model)
    {
        if (ModelState.IsValid)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var usrId = currentUser?.Id;

            var volunteer = await volunteerService.GetByUserIdAsync(usrId);

            var status = await volunteerRequestStatusService.GetByDescriptionAsync("Pending");
            if (currentUser != null)
            {
                var leavingRequest = new VolunteerRequestLeaving
                {
                    Id = Guid.NewGuid().ToString(),
                    VolunteerId = volunteer.Id,
                    StatusId = status.Id,
                    Reason = model.Reason,
                    Date = DateTime.UtcNow
                };

                await volunteerRequestLeavingService.AddAsync(leavingRequest);
                await volunteerRequestLeavingService.SaveChangesAsync();
            }
        }
        
        return RedirectToAction("RequestsLeaving");
    }
    
    [HttpPost]
    public async Task<IActionResult> AcceptLeaving(string requestId)
    {
        var currentRequest = await volunteerRequestLeavingService.GetByIdAsync(requestId);
        if (currentRequest == null) return NotFound();
        
        var currentVolunteer = await volunteerService.GetByIdAsync(currentRequest.VolunteerId);
        if (currentVolunteer == null) return NotFound();
        
        var currentUser = await userManager.FindByIdAsync(currentVolunteer.UserId);
        if (currentUser != null)
        {
            var newStatus = await volunteerRequestStatusService.GetByDescriptionAsync("Accepted");
            
            if ( newStatus == null ) return NotFound();
            
            currentRequest.StatusId = newStatus.Id;
            await volunteerRequestLeavingService.UpdateAsync(currentRequest);
            await volunteerRequestLeavingService.SaveChangesAsync();
            await userManager.RemoveFromRoleAsync(currentUser, "Volunteer");
            await userManager.AddToRoleAsync(currentUser, "ExVolunteer");
            await userManager.AddToRoleAsync(currentUser, "Member");
        }

        return RedirectToAction("RequestsLeaving");
    }
    
    [HttpPost]
    public async Task<IActionResult> RejectLeaving(string leavingRequestId)
    {
        var currentRequest = await volunteerRequestLeavingService.GetByIdAsync(leavingRequestId);
        if (currentRequest == null) return NotFound();
        
        var currentVolunteer = await volunteerService.GetByIdAsync(currentRequest.VolunteerId);
        if (currentVolunteer == null) return NotFound();
        
        var currentUser = await userManager.FindByIdAsync(currentVolunteer.UserId);
        if (currentUser != null)
        {
            var newStatus = await volunteerRequestStatusService.GetByDescriptionAsync("Rejected");
            
            if ( newStatus != null ) currentRequest.StatusId = newStatus.Id;

            await volunteerRequestLeavingService.UpdateAsync(currentRequest);
            await volunteerRequestLeavingService.SaveChangesAsync();
        }

        return RedirectToAction("RequestsLeaving");
    }
    
    [HttpPost]
    public async Task<IActionResult> RequestDetailsLeaving(string requestId)
    {
        var request = await volunteerRequestLeavingService.GetByIdAsync(requestId);
        var volunteer = await volunteerService.GetByIdAsync(request.VolunteerId);
        
        var model = new VolunteerRequestLeavingDetailsModel
        {
            Id = request.Id,
            VolunteerId = request.VolunteerId,
            FirstName = volunteer.FirstName,
            LastName = volunteer.LastName,
            Date = request.Date,
            Reason = request.Reason
        };
        
        return View(model);
    }
    
    public async Task<IActionResult> RequestsLeaving()
    {
        var requests = await volunteerRequestLeavingService.GetAllAsync();
        var requestStatuses = await volunteerRequestStatusService.GetAllAsync();
        
        if (User.IsInRole("Member") || User.IsInRole("Volunteer"))
        {
            var usr = await userManager.GetUserAsync(User);
            var volunteer = await volunteerService.GetByUserIdAsync(usr.Id);
    
            requests = requests.Where(q => q.VolunteerId == volunteer.Id);
        }
        
        var model = new VolunteerRequestsLeavingModel
        {
            Requests = requests,
            RequestStatuses = requestStatuses
        };
        
        return View(model);
    }
    
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

        var question = new VolunteerQuestion()
        {
            Id = Guid.NewGuid().ToString(),
            Date = DateTime.UtcNow,
            QuestionTitle = model.QuestionTitle,
            Question = model.Question,
            VolunteerId = volunteer.Id
        };

        await volunteerQuestionService.AddAsync(question);
        await volunteerQuestionService.SaveChangesAsync();

        return RedirectToAction("Index","Home");
    }
    
    [HttpPost]
    public async Task<IActionResult> QuestionDetails(string questionId)
    {
        var question = await volunteerQuestionService.GetByIdAsync(questionId);
        
        var volunteer = await volunteerService.GetByIdAsync(question.VolunteerId);
        if (volunteer == null)
        {
            return NotFound();
        }

        var answer = await volunteerAnswerService.GetByQuestionIdAsync(question.Id);

        var answeredPerson = await userManager.FindByIdAsync(answer.UserId);

        var model = new VolunteerQuestionDetailsModel
        {
            VolunteerIdentifier = "Amir Karara",
            QuestionTitle = question.QuestionTitle,
            Question = question.Question,
            QuestionDate = question.Date,
            AnsweredPersonIdentifier = answeredPerson.NormalizedEmail,
            Answer = answer.Answer,
            AnswerDate = answer.Date
        };

        return View(model);
    }
    
    public async Task<IActionResult> Questions()
    {
        // Retrieve volunteer questions from the service
        var questions = await volunteerQuestionService.GetAllAsync();

        if (User.IsInRole("Member") || User.IsInRole("Volunteer"))
        {
            var usr = await userManager.GetUserAsync(User);
            var volunteer = await volunteerService.GetByUserIdAsync(usr.Id);
    
            questions = questions.Where(q => q.VolunteerId == volunteer.Id);
        }
        
        // Fill the VolunteerQuestionsModel
        var model = new VolunteerQuestionsModel
        {
            Questions = new List<VolunteerQuestionModel>()
        };

        foreach (var question in questions)
        {
            // Check if the question is answered
            var isAnswered = await volunteerAnswerService.IsQuestionAnswered(question.Id);
        
            // Add the question to the model
            model.Questions.Add(new VolunteerQuestionModel
            {
                Id = question.Id,
                VolunteerId = question.VolunteerId,
                Date = question.Date,
                Question = question.Question,
                IsAnswered = isAnswered
            });
        }

        // Pass the model to the view
        return View(model);
    }
    
    public async Task<IActionResult> QuestionAnswerForm(string questionId)
    {
        var question = await volunteerQuestionService.GetByIdAsync(questionId);
    
        var model = new VolunteerAnswerModel()
        {
            QuestionId = question.Id,
            Answer = "",
            Question = question.Question,
            QuestionTitle = question.QuestionTitle
        };
        
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> QuestionAnswerForm(VolunteerAnswerModel model)
    {
        var user = await userManager.GetUserAsync(User);

        if (user == null) return NotFound();

        var answer = new VolunteerAnswer()
        {
            Id = Guid.NewGuid().ToString(),
            UserId = user.Id,
            Date = DateTime.UtcNow,
            QuestionId = model.QuestionId,
            Answer = model.Answer
        };

        await volunteerAnswerService.AddAsync(answer);
        await volunteerAnswerService.SaveChangesAsync();

        return RedirectToAction("Questions", "Volunteer");
    }
}
