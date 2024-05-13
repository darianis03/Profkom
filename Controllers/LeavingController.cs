using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profkom.Data;
using Profkom.Models;
using Profkom.Services;

namespace Profkom.Controllers;

public class LeavingController(
    VolunteerService volunteerService,
    VolunteerRequestStatusService volunteerRequestStatusService,
    VolunteerRequestLeavingService volunteerRequestLeavingService,
    UserManager<AppUser> userManager
    ) : Controller
{
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
            if (currentUser != null && volunteer != null && status != null)
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
        if (await UpdateRequestLeavingStatusAsync(requestId, "Accepted"))
        {
            return RedirectToAction("RequestsLeaving");
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> RejectLeaving(string leavingRequestId)
    {
        if (await UpdateRequestLeavingStatusAsync(leavingRequestId, "Rejected"))
        {
            return RedirectToAction("RequestsLeaving");
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> RequestDetailsLeaving(string requestId)
    {
        var request = await volunteerRequestLeavingService.GetByIdAsync(requestId);
        var volunteer = await volunteerService.GetByIdAsync(request?.VolunteerId);

        if (request == null || volunteer == null) return NotFound();

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

    private async Task<bool> UpdateRequestLeavingStatusAsync(string requestId, string statusDescription)
    {
        var currentRequest = await volunteerRequestLeavingService.GetByIdAsync(requestId);
        if (currentRequest == null) return false;

        var newStatus = await volunteerRequestStatusService.GetByDescriptionAsync(statusDescription);
        if (newStatus == null) return false;

        currentRequest.StatusId = newStatus.Id;
        await volunteerRequestLeavingService.UpdateAsync(currentRequest);
        await volunteerRequestLeavingService.SaveChangesAsync();

        return true;
    }
}
