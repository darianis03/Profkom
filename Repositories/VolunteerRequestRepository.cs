using Microsoft.EntityFrameworkCore;
using Profkom.Data;

namespace Profkom.Repositories;

public class VolunteerRequestRepository
{
    private readonly VolunteerDbContext _context;

    public VolunteerRequestRepository(VolunteerDbContext context)
    {
        _context = context;
    }

    public VolunteerRequest GetById(string id)
    {
        return _context.VolunteerRequests
            .Include(vr => vr.Volunteer) // Eager loading of Volunteer navigation property (optional)
            .FirstOrDefault(vr => vr.Id == id);
    }

    public async Task<VolunteerRequest> GetByIdAsync(string id)
    {
        return await _context.VolunteerRequests
            .Include(vr => vr.Volunteer) // Eager loading of Volunteer navigation property (optional)
            .FirstOrDefaultAsync(vr => vr.Id == id);
    }

    public IEnumerable<VolunteerRequest> GetAll()
    {
        return _context.VolunteerRequests
            .Include(vr => vr.Volunteer); // Eager loading of Volunteer navigation property (optional)
    }

    public async Task<IEnumerable<VolunteerRequest>> GetAllAsync()
    {
        return await _context.VolunteerRequests
            .Include(vr => vr.Volunteer) // Eager loading of Volunteer navigation property (optional)
            .ToListAsync();
    }

    public void Add(VolunteerRequest entity)
    {
        _context.VolunteerRequests.Add(entity);
    }

    public async Task AddAsync(VolunteerRequest entity)
    {
        await _context.VolunteerRequests.AddAsync(entity);
    }

    public void Update(VolunteerRequest entity)
    {
        _context.VolunteerRequests.Update(entity);
    }

    public async Task UpdateAsync(VolunteerRequest entity)
    {
        _context.VolunteerRequests.Update(entity);
    }

    public void Delete(string id)
    {
        var request = GetById(id);
        if (request != null)
        {
            _context.VolunteerRequests.Remove(request);
        }
    }

    public async Task DeleteAsync(string id)
    {
        var request = await GetByIdAsync(id);
        if (request != null)
        {
            _context.VolunteerRequests.Remove(request);
        }
    }
}
