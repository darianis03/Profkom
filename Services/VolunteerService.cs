using Profkom.Data;
using Profkom.Repositories;

namespace Profkom.Services;

public class VolunteerService(
    VolunteerRepository volunteerRepository,
    VolunteerDbContext context
    )
{
    public Volunteer? GetById(string id)
    {
        return volunteerRepository.GetById(id);
    }

    public async Task<Volunteer?> GetByIdAsync(string id)
    {
        return await volunteerRepository.GetByIdAsync(id);
    }

    public IEnumerable<Volunteer?> GetAll()
    {
        return volunteerRepository.GetAll();
    }

    public async Task<IEnumerable<Volunteer?>> GetAllAsync()
    {
        return await volunteerRepository.GetAllAsync();
    }

    public void Add(Volunteer volunteer)
    {
        volunteerRepository.Add(volunteer);
    }

    public async Task AddAsync(Volunteer volunteer)
    {
        await volunteerRepository.AddAsync(volunteer);
    }

    public void Update(Volunteer volunteer)
    {
        volunteerRepository.Update(volunteer);
    }

    public async Task UpdateAsync(Volunteer volunteer)
    {
        await volunteerRepository.UpdateAsync(volunteer);
    }

    public void Delete(string id)
    {
        volunteerRepository.Delete(id);
    }

    public async Task DeleteAsync(string id)
    {
        await volunteerRepository.DeleteAsync(id);
    }

    public Volunteer? GetByUserId(string userId)
    {
        return volunteerRepository.GetByUserId(userId);
    }

    public async Task<Volunteer?> GetByUserIdAsync(string userId)
    {
        return await volunteerRepository.GetByUserIdAsync(userId);
    }
    
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
