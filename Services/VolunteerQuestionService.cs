using Microsoft.EntityFrameworkCore;
using Profkom.Data;
using Profkom.Repositories;

namespace Profkom.Services;
public class VolunteerQuestionService(
    VolunteerQuestionRepository volunteerQuestionRepository,
    VolunteerDbContext context
    ) {
    public VolunteerQuestion GetById(string id)
    {
        return volunteerQuestionRepository.GetById(id);
    }

    public async Task<VolunteerQuestion> GetByIdAsync(string id)
    {
        return await volunteerQuestionRepository.GetByIdAsync(id);
    }

    public IEnumerable<VolunteerQuestion> GetAll()
    {
        return volunteerQuestionRepository.GetAll();
    }

    public async Task<IEnumerable<VolunteerQuestion>> GetAllAsync()
    {
        return await volunteerQuestionRepository.GetAllAsync();
    }

    public void Add(VolunteerQuestion volunteerRequest)
    {
        volunteerQuestionRepository.Add(volunteerRequest);
    }

    public async Task AddAsync(VolunteerQuestion volunteerRequest)
    {
        await volunteerQuestionRepository.AddAsync(volunteerRequest);
    }

    public void Update(VolunteerQuestion volunteerRequest)
    {
        volunteerQuestionRepository.Update(volunteerRequest);
    }

    public async Task UpdateAsync(VolunteerQuestion volunteerRequest)
    {
        await volunteerQuestionRepository.UpdateAsync(volunteerRequest);
    }

    public void Delete(string id)
    {
        volunteerQuestionRepository.Delete(id);
    }

    public async Task DeleteAsync(string id)
    {
        await volunteerQuestionRepository.DeleteAsync(id);
    }
    

    public IEnumerable<VolunteerQuestion> GetByVolunteerId(string volunteerId)
    {
        return context.VolunteerQuestions.Where(vr => vr.VolunteerId == volunteerId)
            .Include(vr => vr.Volunteer);
    }

    public async Task<VolunteerQuestion?> GetByVolunteerIdAsync(string volunteerId)
    {
        return await context.VolunteerQuestions
            .Include(vr => vr.Volunteer)
            .FirstOrDefaultAsync(vr => vr.VolunteerId == volunteerId);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
