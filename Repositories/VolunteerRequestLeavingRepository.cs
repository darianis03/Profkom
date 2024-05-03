using Microsoft.EntityFrameworkCore;
using Profkom.Data;

namespace Profkom.Repositories;

public class VolunteerRequestLeavingRepository
{
    private readonly VolunteerDbContext _context;

    public VolunteerRequestLeavingRepository(VolunteerDbContext context)
    {
        _context = context;
    }

    public VolunteerRequestLeaving GetById(string id)
    {
        return _context.VolunteerRequestLeaving
            .Include(vr => vr.Volunteer) // Eager loading of Volunteer navigation property (optional)
            .FirstOrDefault(vr => vr.Id == id);
    }

    public async Task<VolunteerRequestLeaving> GetByIdAsync(string id)
    {
        return await _context.VolunteerRequestLeaving
            .Include(vr => vr.Volunteer) // Eager loading of Volunteer navigation property (optional)
            .FirstOrDefaultAsync(vr => vr.Id == id);
    }

    public IEnumerable<VolunteerRequestLeaving> GetAll()
    {
        return _context.VolunteerRequestLeaving
            .Include(vr => vr.Volunteer); // Eager loading of Volunteer navigation property (optional)
    }

    public async Task<IEnumerable<VolunteerRequestLeaving>> GetAllAsync()
    {
        return await _context.VolunteerRequestLeaving
            .Include(vr => vr.Volunteer) // Eager loading of Volunteer navigation property (optional)
            .ToListAsync();
    }

    public void Add(VolunteerRequestLeaving entity)
    {
        _context.VolunteerRequestLeaving.Add(entity);
    }

    public async Task AddAsync(VolunteerRequestLeaving entity)
    {
        await _context.VolunteerRequestLeaving.AddAsync(entity);
    }

    public void Update(VolunteerRequestLeaving entity)
    {
        _context.VolunteerRequestLeaving.Update(entity);
    }

    public async Task UpdateAsync(VolunteerRequestLeaving entity)
    {
        _context.VolunteerRequestLeaving.Update(entity);
    }

    public void Delete(string id)
    {
        var request = GetById(id);
        if (request != null)
        {
            _context.VolunteerRequestLeaving.Remove(request);
        }
    }

    public async Task DeleteAsync(string id)
    {
        var request = await GetByIdAsync(id);
        if (request != null)
        {
            _context.VolunteerRequestLeaving.Remove(request);
        }
    }
}
