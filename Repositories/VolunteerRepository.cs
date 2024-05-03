using Microsoft.EntityFrameworkCore;
using Profkom.Data;

namespace Profkom.Repositories;

public class VolunteerRepository(
    VolunteerDbContext context
    ) {
    public Volunteer? GetById(string id)
    {
        return context.Volunteers.Find(id);
    }

    public async Task<Volunteer?> GetByIdAsync(string id)
    {
        return await context.Volunteers.FindAsync(id);
    }

    public IEnumerable<Volunteer?> GetAll()
    {
        return context.Volunteers;
    }

    public async Task<IEnumerable<Volunteer?>> GetAllAsync()
    {
        return await context.Volunteers.ToListAsync();
    }

    public void Add(Volunteer? entity)
    {
        context.Volunteers.Add(entity);
    }

    public async Task AddAsync(Volunteer? entity)
    {
        await context.Volunteers.AddAsync(entity);
    }

    public void Update(Volunteer? entity)
    {
        context.Volunteers.Update(entity);
    }

    public async Task UpdateAsync(Volunteer? entity)
    {
        context.Volunteers.Update(entity);
    }

    public void Delete(string id)
    {
        var volunteer = GetById(id);
        context.Volunteers.Remove(volunteer);
    }

    public async Task DeleteAsync(string id)
    {
        var volunteer = await GetByIdAsync(id);
        if (volunteer != null)
        {
            context.Volunteers.Remove(volunteer);
        }
    }
    
    public Volunteer? GetByUserId(string userId)
    {
        return context.Volunteers.FirstOrDefault(v => v.UserId == userId);
    }

    public async Task<Volunteer?> GetByUserIdAsync(string userId)
    {
        return await context.Volunteers
            .Where(vr => vr.UserId == userId)
            .FirstOrDefaultAsync();
    }
}
