using Microsoft.EntityFrameworkCore;
using Profkom.Data;

namespace Profkom.Repositories;

public class VolunteerRequestStatusRepository(
    VolunteerDbContext context
    )
{
    public VolunteerRequestStatus GetById(string id)
    {
        return context.VolunteerRequestStatuses
            .FirstOrDefault(vr => vr.Id == id);
    }

    public async Task<VolunteerRequestStatus?> GetByIdAsync(string id)
    {
        return await context.VolunteerRequestStatuses
            .FirstOrDefaultAsync(vr => vr.Id == id);
    }

    public IEnumerable<VolunteerRequestStatus?> GetAll()
    {
        return context.VolunteerRequestStatuses.ToList();
    }

    public async Task<IEnumerable<VolunteerRequestStatus?>> GetAllAsync()
    {
        return await context.VolunteerRequestStatuses.ToListAsync();
    }

    public void Add(VolunteerRequestStatus? entity)
    {
        context.VolunteerRequestStatuses.Add(entity);
    }

    public async Task AddAsync(VolunteerRequestStatus? entity)
    {
        await context.VolunteerRequestStatuses.AddAsync(entity);
    }

    public void Update(VolunteerRequestStatus? entity)
    {
        context.VolunteerRequestStatuses.Update(entity);
    }

    public async Task UpdateAsync(VolunteerRequestStatus? entity)
    {
        context.VolunteerRequestStatuses.Update(entity);
    }

    public void Delete(string id)
    {
        var request = GetById(id);
        if (request != null)
        {
            context.VolunteerRequestStatuses.Remove(request);
        }
    }

    public async Task DeleteAsync(string id)
    {
        var request = await GetByIdAsync(id);
        if (request != null)
        {
            context.VolunteerRequestStatuses.Remove(request);
        }
    }
}
