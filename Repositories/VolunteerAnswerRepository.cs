using Microsoft.EntityFrameworkCore;
using Profkom.Data;

namespace Profkom.Repositories;

public class VolunteerAnswerRepository
{
    private readonly VolunteerDbContext _context;

    public VolunteerAnswerRepository(VolunteerDbContext context)
    {
        _context = context;
    }

    public VolunteerAnswer GetById(string id)
    {
        return _context.VolunteerAnswers
            .Include(vr => vr.Question)
            .FirstOrDefault(vr => vr.Id == id);
    }

    public async Task<VolunteerAnswer> GetByIdAsync(string id)
    {
        return await _context.VolunteerAnswers
            .Include(vr => vr.Question) // Eager loading of Volunteer navigation property (optional)
            .FirstOrDefaultAsync(vr => vr.Id == id);
    }

    public IEnumerable<VolunteerAnswer> GetAll()
    {
        return _context.VolunteerAnswers
            .Include(vr => vr.Question); // Eager loading of Volunteer navigation property (optional)
    }

    public async Task<IEnumerable<VolunteerAnswer>> GetAllAsync()
    {
        return await _context.VolunteerAnswers
            .Include(vr => vr.Question) // Eager loading of Volunteer navigation property (optional)
            .ToListAsync();
    }

    public void Add(VolunteerAnswer entity)
    {
        _context.VolunteerAnswers.Add(entity);
    }

    public async Task AddAsync(VolunteerAnswer entity)
    {
        await _context.VolunteerAnswers.AddAsync(entity);
    }

    public void Update(VolunteerAnswer entity)
    {
        _context.VolunteerAnswers.Update(entity);
    }

    public async Task UpdateAsync(VolunteerAnswer entity)
    {
        _context.VolunteerAnswers.Update(entity);
    }

    public void Delete(string id)
    {
        var request = GetById(id);
        if (request != null)
        {
            _context.VolunteerAnswers.Remove(request);
        }
    }

    public async Task DeleteAsync(string id)
    {
        var request = await GetByIdAsync(id);
        if (request != null)
        {
            _context.VolunteerAnswers.Remove(request);
        }
    }
}
