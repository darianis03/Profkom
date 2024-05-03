using Microsoft.EntityFrameworkCore;

namespace Profkom.Data;

public class VolunteerDbContext : DbContext
{
    public VolunteerDbContext(DbContextOptions<VolunteerDbContext> options) : base(options)
    {
    }
    
    public DbSet<Volunteer?> Volunteers { get; set; }
    public DbSet<VolunteerRequest> VolunteerRequests { get; set; }
    public DbSet<VolunteerRequestStatus?> VolunteerRequestStatuses { get; set; }
    public DbSet<VolunteerRequestLeaving?> VolunteerRequestLeaving { get; set; }
    public DbSet<VolunteerQuestion?> VolunteerQuestions { get; set; }
    public DbSet<VolunteerAnswer?> VolunteerAnswers { get; set; }
}