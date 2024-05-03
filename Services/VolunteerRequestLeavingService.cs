using Microsoft.EntityFrameworkCore;
using Profkom.Data;
using Profkom.Repositories;

namespace Profkom.Services;
public class VolunteerRequestLeavingService(
    VolunteerRequestLeavingRepository volunteerRequestLeavingRepository,
    VolunteerDbContext context
    ) {
    public VolunteerRequestLeaving GetById(string id)
    {
        return volunteerRequestLeavingRepository.GetById(id);
    }

    public async Task<VolunteerRequestLeaving> GetByIdAsync(string id)
    {
        return await volunteerRequestLeavingRepository.GetByIdAsync(id);
    }

    public IEnumerable<VolunteerRequestLeaving> GetAll()
    {
        return volunteerRequestLeavingRepository.GetAll();
    }

    public async Task<IEnumerable<VolunteerRequestLeaving>> GetAllAsync()
    {
        return await volunteerRequestLeavingRepository.GetAllAsync();
    }

    public void Add(VolunteerRequestLeaving volunteerRequest)
    {
        volunteerRequestLeavingRepository.Add(volunteerRequest);
    }

    public async Task AddAsync(VolunteerRequestLeaving volunteerRequest)
    {
        await volunteerRequestLeavingRepository.AddAsync(volunteerRequest);
    }

    public void Update(VolunteerRequestLeaving volunteerRequest)
    {
        volunteerRequestLeavingRepository.Update(volunteerRequest);
    }

    public async Task UpdateAsync(VolunteerRequestLeaving volunteerRequest)
    {
        await volunteerRequestLeavingRepository.UpdateAsync(volunteerRequest);
    }

    public void Delete(string id)
    {
        volunteerRequestLeavingRepository.Delete(id);
    }

    public async Task DeleteAsync(string id)
    {
        await volunteerRequestLeavingRepository.DeleteAsync(id);
    }
    

    public IEnumerable<VolunteerRequestLeaving> GetByVolunteerId(string volunteerId)
    {
        return context.VolunteerRequestLeaving.Where(vr => vr.VolunteerId == volunteerId)
            .Include(vr => vr.Volunteer);
    }

    public async Task<VolunteerRequestLeaving?> GetByVolunteerIdAsync(string volunteerId)
    {
        return await context.VolunteerRequestLeaving
            .Include(vr => vr.Volunteer)
            .FirstOrDefaultAsync(vr => vr.VolunteerId == volunteerId);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
