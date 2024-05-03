using Microsoft.EntityFrameworkCore;
using Profkom.Data;
using Profkom.Repositories;

namespace Profkom.Services;
public class VolunteerAnswerService(
    VolunteerAnswerRepository volunteerAnswerRepository,
    VolunteerDbContext context
    ) {
    public VolunteerAnswer GetById(string id)
    {
        return volunteerAnswerRepository.GetById(id);
    }

    public async Task<VolunteerAnswer> GetByIdAsync(string id)
    {
        return await volunteerAnswerRepository.GetByIdAsync(id);
    }

    public IEnumerable<VolunteerAnswer> GetAll()
    {
        return volunteerAnswerRepository.GetAll();
    }

    public async Task<IEnumerable<VolunteerAnswer>> GetAllAsync()
    {
        return await volunteerAnswerRepository.GetAllAsync();
    }

    public void Add(VolunteerAnswer volunteerRequest)
    {
        volunteerAnswerRepository.Add(volunteerRequest);
    }

    public async Task AddAsync(VolunteerAnswer volunteerRequest)
    {
        await volunteerAnswerRepository.AddAsync(volunteerRequest);
    }

    public void Update(VolunteerAnswer volunteerRequest)
    {
        volunteerAnswerRepository.Update(volunteerRequest);
    }

    public async Task UpdateAsync(VolunteerAnswer volunteerRequest)
    {
        await volunteerAnswerRepository.UpdateAsync(volunteerRequest);
    }

    public void Delete(string id)
    {
        volunteerAnswerRepository.Delete(id);
    }

    public async Task DeleteAsync(string id)
    {
        await volunteerAnswerRepository.DeleteAsync(id);
    }
    

    public IEnumerable<VolunteerAnswer> GetByQuestionId(string questionId)
    {
        return context.VolunteerAnswers.Where(vr => vr.QuestionId == questionId)
            .Include(vr => vr.Question);
    }

    public async Task<VolunteerAnswer?> GetByQuestionIdAsync(string questionId)
    {
        return await context.VolunteerAnswers
            .Where(vr => vr.QuestionId == questionId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsQuestionAnswered(string questionId)
    {
        var answer = await context.VolunteerAnswers.FirstOrDefaultAsync(q => q.QuestionId == questionId);
        return answer != null;
    }
    
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
