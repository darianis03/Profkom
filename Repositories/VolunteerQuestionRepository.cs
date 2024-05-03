using Microsoft.EntityFrameworkCore;
using Profkom.Data;

namespace Profkom.Repositories;

public class VolunteerQuestionRepository
{
    private readonly VolunteerDbContext _context;

    public VolunteerQuestionRepository(VolunteerDbContext context)
    {
        _context = context;
    }

    public VolunteerQuestion GetById(string id)
    {
        return _context.VolunteerQuestions
            .Include(vr => vr.Volunteer) // Eager loading of Volunteer navigation property (optional)
            .FirstOrDefault(vr => vr.Id == id);
    }

    public async Task<VolunteerQuestion> GetByIdAsync(string id)
    {
        return await _context.VolunteerQuestions
            .Include(vr => vr.Volunteer) // Eager loading of Volunteer navigation property (optional)
            .FirstOrDefaultAsync(vr => vr.Id == id);
    }

    public IEnumerable<VolunteerQuestion> GetAll()
    {
        return _context.VolunteerQuestions
            .Include(vr => vr.Volunteer); // Eager loading of Volunteer navigation property (optional)
    }

    public async Task<IEnumerable<VolunteerQuestion>> GetAllAsync()
    {
        return await _context.VolunteerQuestions
            .Include(vr => vr.Volunteer) // Eager loading of Volunteer navigation property (optional)
            .ToListAsync();
    }

    public void Add(VolunteerQuestion entity)
    {
        _context.VolunteerQuestions.Add(entity);
    }

    public async Task AddAsync(VolunteerQuestion entity)
    {
        await _context.VolunteerQuestions.AddAsync(entity);
    }

    public void Update(VolunteerQuestion entity)
    {
        _context.VolunteerQuestions.Update(entity);
    }

    public async Task UpdateAsync(VolunteerQuestion entity)
    {
        _context.VolunteerQuestions.Update(entity);
    }

    public void Delete(string id)
    {
        var request = GetById(id);
        if (request != null)
        {
            _context.VolunteerQuestions.Remove(request);
        }
    }

    public async Task DeleteAsync(string id)
    {
        var request = await GetByIdAsync(id);
        if (request != null)
        {
            _context.VolunteerQuestions.Remove(request);
        }
    }
}
