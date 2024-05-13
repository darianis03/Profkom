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
    UserManager<AppUser> userManager
) : Controller
{
    [Authorize]
    public IActionResult Apply()
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
                var volunteer = new Volunteer
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

        var currentVolunteer = await GetVolunteerByIdAsync(currentRequest.VolunteerId);
        var currentUser = await GetUserByVolunteerIdAsync(currentRequest.VolunteerId);

        if (currentUser != null)
        {
            if (await UpdateRequestStatusAsync(requestId, "Accepted"))
            {
                await UpdateUserRolesAsync(currentUser, "Member", "Volunteer");
            }
        }

        return RedirectToAction("Requests");
    }

    [Authorize]
    public async Task<IActionResult> RejectRequest(string requestId)
    {
        var currentRequest = await volunteerRequestService.GetByIdAsync(requestId);
        if (currentRequest == null) return NotFound();

        if (await UpdateRequestStatusAsync(requestId, "Rejected"))
        {
            return RedirectToAction("Requests");
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> RequestDetails(string requestId)
    {
        var request = await volunteerRequestService.GetByIdAsync(requestId);
        var volunteer = await GetVolunteerByIdAsync(request.VolunteerId);

        var model = new VolunteerRequestDetailsModel
        {
            Id = request.Id,
            VolunteerId = request.VolunteerId,
            FirstName = volunteer.FirstName,
            LastName = volunteer.LastName,
            Date = request.Date
        };

        return View(model);
    }private async Task<Volunteer> GetVolunteerByIdAsync(string volunteerId)
    {
        return await volunteerService.GetByIdAsync(volunteerId);
    }

    private async Task<AppUser> GetUserByVolunteerIdAsync(string volunteerId)
    {
        var volunteer = await GetVolunteerByIdAsync(volunteerId);
        if (volunteer == null) return null;

        return await userManager.FindByIdAsync(volunteer.UserId);
    }

    private async Task<VolunteerRequestStatus> GetStatusByDescriptionAsync(string description)
    {
        return await volunteerRequestStatusService.GetByDescriptionAsync(description);
    }

    private async Task<bool> UpdateRequestStatusAsync(string requestId, string statusDescription)
    {
        var request = await volunteerRequestService.GetByIdAsync(requestId);
        if (request == null) return false;

        var newStatus = await GetStatusByDescriptionAsync(statusDescription);
        if (newStatus == null) return false;

        request.StatusId = newStatus.Id;
        await volunteerRequestService.UpdateAsync(request);
        await volunteerRequestService.SaveChangesAsync();

        return true;
    }

    private async Task<bool> UpdateUserRolesAsync(AppUser user, string removeRole, string addRole)
    {
        if (user == null) return false;

        await userManager.RemoveFromRoleAsync(user, removeRole);
        await userManager.AddToRoleAsync(user, addRole);

        return true;
    }
}