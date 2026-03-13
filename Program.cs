using jobs_service_backend.Data;
using Microsoft.EntityFrameworkCore;
// הוסיפי את ה-usings האלו כדי שהקוד יזהה את השכבות החדשות
using JobsService.BLL.Repositories;
using JobsService.BLL.Services;
using FluentValidation.AspNetCore;
using FluentValidation;
using JobsService.BLL.Validators;

var builder = WebApplication.CreateBuilder(args);

// --- 1. רישום קונטרולרים ---
builder.Services.AddControllers();

// --- 2. הגדרת בסיס הנתונים ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                        ?? "Server=(localdb)\\mssqllocaldb;Database=JobsServiceDb;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- 3. רישום השכבות שלנו (Dependency Injection) ---
// Scoped אומר שהאובייקט ייווצר מחדש בכל בקשת HTTP
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobService, JobService>();

// --- 4. רישום AutoMapper ---
// הוא סורק אוטומטית את כל ה-Profiles שהגדרנו ב-BLL
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// --- 5. רישום FluentValidation ---
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateJobDtoValidator>();

// --- 6. Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 7. הרצת ה-Seed (נתונים ראשוניים לבדיקה) ---
// הוספתי כאן לוגיקה שיוצרת נתונים אם הדאטהבייס ריק
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    // קריאה לפונקציית ה-Seed (ניצור אותה מיד)
    DbInitializer.Seed(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization(); // חשוב אם תוסיפי בהמשך אבטחה
app.MapControllers();

app.Run();