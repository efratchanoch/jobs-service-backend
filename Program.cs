using jobs_service_backend.Data;
using Microsoft.EntityFrameworkCore;
using jobs_service_backend.BLL.Repositories.Repositories; 
using jobs_service_backend.BLL.Repositories.Services;     
using FluentValidation.AspNetCore;
using FluentValidation;
using jobs_service_backend.BLL.Validators;

var builder = WebApplication.CreateBuilder(args);

// --- 1. רישום קונטרולרים ---
builder.Services.AddControllers();

// --- 2. הגדרת בסיס הנתונים ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                        ?? "Server=(localdb)\\mssqllocaldb;Database=jobs_service_backendDb;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- 3. רישום השכבות (DI) ---
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
builder.Services.AddScoped<IJobService, JobService>();

// --- 4. רישום AutoMapper ---
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// --- 5. רישום FluentValidation ---
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateJobDtoValidator>();

// --- 6. Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 7. הגדרות הרצה (ללא ה-Seed הבעייתי) ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();