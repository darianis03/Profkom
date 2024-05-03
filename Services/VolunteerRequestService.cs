using Microsoft.EntityFrameworkCore;
using Profkom.Data;
using Profkom.Repositories;

namespace Profkom.Services;
public class VolunteerRequestService(
    VolunteerRequestRepository volunteerRequestRepository,
    VolunteerDbContext context
    ) {
    public VolunteerRequest GetById(string id)
    {
        return volunteerRequestRepository.GetById(id);
    }

    public async Task<VolunteerRequest> GetByIdAsync(string id)
    {
        return await volunteerRequestRepository.GetByIdAsync(id);
    }

    public IEnumerable<VolunteerRequest> GetAll()
    {
        return volunteerRequestRepository.GetAll();
    }

    public async Task<IEnumerable<VolunteerRequest>> GetAllAsync()
    {
        return await volunteerRequestRepository.GetAllAsync();
    }

    public void Add(VolunteerRequest volunteerRequest)
    {
        volunteerRequestRepository.Add(volunteerRequest);
    }

    public async Task AddAsync(VolunteerRequest volunteerRequest)
    {
        await volunteerRequestRepository.AddAsync(volunteerRequest);
    }

    public void Update(VolunteerRequest volunteerRequest)
    {
        volunteerRequestRepository.Update(volunteerRequest);
    }

    public async Task UpdateAsync(VolunteerRequest volunteerRequest)
    {
        await volunteerRequestRepository.UpdateAsync(volunteerRequest);
    }

    public void Delete(string id)
    {
        volunteerRequestRepository.Delete(id);
    }

    public async Task DeleteAsync(string id)
    {
        await volunteerRequestRepository.DeleteAsync(id);
    }
    

    public IEnumerable<VolunteerRequest> GetByVolunteerId(string volunteerId)
    {
        return context.VolunteerRequests.Where(vr => vr.VolunteerId == volunteerId)
            .Include(vr => vr.Volunteer);
    }

    public async Task<VolunteerRequest?> GetByVolunteerIdAsync(string volunteerId)
    {
        return await context.VolunteerRequests
            .Include(vr => vr.Volunteer)
            .FirstOrDefaultAsync(vr => vr.VolunteerId == volunteerId);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
