using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_liger.Data;
using wt_lab7_7postgres_efcore_liger.Models;

var builder = WebApplication.CreateBuilder(args);

// Подключение к БД (с учетом DATABASE_URL для Render)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseUrl = builder.Configuration["DATABASE_URL"];

if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);
    connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={Uri.UnescapeDataString(userInfo[0])};Password={Uri.UnescapeDataString(userInfo[1])};SSL Mode=Disable";
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Swagger (Способ 3)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Автоматическое применение миграций
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ===== ЭНДПОИНТЫ =====

app.MapGet("/", () => "Minimal API работает с PostgreSQL!");
app.MapGet("/api/info", () => new { Application = "Web Technologies", Technology = "ASP.NET Core Minimal API + PostgreSQL", Version = "2.0" });

app.MapGet("/api/students", async (AppDbContext db) =>
{
    var students = await db.Students.ToListAsync();
    return Results.Ok(students);
});

app.MapGet("/api/students/{id}", async (int id, AppDbContext db) =>
{
    var student = await db.Students.FirstOrDefaultAsync(s => s.Id == id);
    if (student == null) return Results.NotFound();
    return Results.Ok(student);
});

app.MapGet("/api/students/search", async (string? search, AppDbContext db) =>
{
    var query = db.Students.AsQueryable();
    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(s => s.FullName.ToLower().Contains(search.ToLower()) || s.Specialty.ToLower().Contains(search.ToLower()));
    }
    var students = await query.ToListAsync();
    return Results.Ok(students);
});

app.MapPost("/api/students", async (Student student, AppDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(student.FullName)) return Results.BadRequest("ФИО обязательно.");
    if (student.Course < 1 || student.Course > 4) return Results.BadRequest("Курс от 1 до 4.");
    if (student.AverageGrade < 0 || student.AverageGrade > 10) return Results.BadRequest("Средний балл от 0 до 10.");

    student.Id = 0;
    student.CreatedAt = DateTime.UtcNow;

    db.Students.Add(student);
    await db.SaveChangesAsync();
    return Results.Created($"/api/students/{student.Id}", student);
});

app.MapPut("/api/students/{id}", async (int id, Student updatedStudent, AppDbContext db) =>
{
    var student = await db.Students.FirstOrDefaultAsync(s => s.Id == id);
    if (student == null) return Results.NotFound();

    student.FullName = updatedStudent.FullName;
    student.Specialty = updatedStudent.Specialty;
    student.Course = updatedStudent.Course;
    student.AverageGrade = updatedStudent.AverageGrade;
    student.IsEnrolled = updatedStudent.IsEnrolled;

    await db.SaveChangesAsync();
    return Results.Ok(student);
});

app.MapDelete("/api/students/{id}", async (int id, AppDbContext db) =>
{
    var student = await db.Students.FirstOrDefaultAsync(s => s.Id == id);
    if (student == null) return Results.NotFound();

    db.Students.Remove(student);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// LINQ-запросы
app.MapGet("/api/students/top", async (AppDbContext db) =>
{
    var students = await db.Students.Where(s => s.AverageGrade > 4.0).ToListAsync();
    return Results.Ok(students);
});

app.MapGet("/api/students/sorted", async (AppDbContext db) =>
{
    var students = await db.Students.OrderBy(s => s.FullName).ToListAsync();
    return Results.Ok(students);
});

app.MapGet("/api/students/enrolled", async (AppDbContext db) =>
{
    var students = await db.Students.Where(s => s.IsEnrolled).OrderBy(s => s.Course).ToListAsync();
    return Results.Ok(students);
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");