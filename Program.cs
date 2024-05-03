using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Profkom.Data;
using Profkom.Repositories;
using Profkom.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("ConnectionString") ?? "";

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<VolunteerDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<VolunteerRepository>();
builder.Services.AddScoped<VolunteerService>();

builder.Services.AddScoped<VolunteerRequestRepository>();
builder.Services.AddScoped<VolunteerRequestService>();

builder.Services.AddScoped<VolunteerRequestStatusRepository>();
builder.Services.AddScoped<VolunteerRequestStatusService>();

builder.Services.AddScoped<VolunteerQuestionRepository>();
builder.Services.AddScoped<VolunteerQuestionService>();

builder.Services.AddScoped<VolunteerAnswerRepository>();
builder.Services.AddScoped<VolunteerAnswerService>();

builder.Services.AddScoped<VolunteerRequestLeavingRepository>();
builder.Services.AddScoped<VolunteerRequestLeavingService>();

builder.Services
    .AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddSignInManager<SignInManager<AppUser>>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<RoleManager<AppRole>>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();