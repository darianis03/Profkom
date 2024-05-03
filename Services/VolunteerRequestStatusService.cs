using Profkom.Data;
using Profkom.Repositories;

namespace Profkom.Services;

public class VolunteerRequestStatusService(
    VolunteerRequestStatusRepository repository,
    VolunteerDbContext context
    )
{
    public VolunteerRequestStatus GetById(string id)
    {
        return repository.GetById(id);
    }

    public async Task<VolunteerRequestStatus?> GetByIdAsync(string id)
    {
        return await repository.GetByIdAsync(id);
    }

    public IEnumerable<VolunteerRequestStatus?> GetAll()
    {
        return repository.GetAll();
    }

    public async Task<IEnumerable<VolunteerRequestStatus?>> GetAllAsync()
    {
        return await repository.GetAllAsync();
    }
        
    public IEnumerable<VolunteerRequestStatus> GetByDescription(string description)
    {
        return repository.GetAll().Where(s => s.Description.Contains(description));
    }

    public async Task<VolunteerRequestStatus?> GetByDescriptionAsync(string description)
    {
        var getAll = await repository.GetAllAsync();
        return getAll.FirstOrDefault(s => s.Description.Contains(description));
    }


    public async Task AddAsync(VolunteerRequestStatus entity)
    {
        await repository.AddAsync(entity);
    }
    
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}