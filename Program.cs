using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_liger.Data;
using wt_lab7_7postgres_efcore_liger.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Подключение к БД (с учетом DATABASE_URL для Render)
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Автоматическое применение миграций при запуске
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

// ===== ЭНДПОИНТЫ (CRUD через EF Core) =====

app.MapGet("/", () => "Minimal API работает с PostgreSQL!");

app.MapGet("/api/info", () => new { Application = "Web Technologies", Technology = "ASP.NET Core Minimal API + PostgreSQL", Version = "2.0" });

// Получение всех
app.MapGet("/api/students", async (AppDbContext db) =>
{
    var students = await db.Students.ToListAsync();
    return Results.Ok(students);
});

// Получение по ID
app.MapGet("/api/students/{id}", async (int id, AppDbContext db) =>
{
    var student = await db.Students.FirstOrDefaultAsync(s => s.Id == id);
    if (student == null) return Results.NotFound();
    return Results.Ok(student);
});

// Поиск
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

// Создание
app.MapPost("/api/students", async (Student student, AppDbContext db) =>
{
    // Валидация
    if (string.IsNullOrWhiteSpace(student.FullName)) return Results.BadRequest("ФИО обязательно.");
    if (student.Course < 1 || student.Course > 4) return Results.BadRequest("Курс от 1 до 4.");

    student.Id = 0; // EF Core сгенерирует ID автоматически
    student.CreatedAt = DateTime.UtcNow; // Устанавливаем дату создания

    db.Students.Add(student);
    await db.SaveChangesAsync();
    return Results.Created($"/api/students/{student.Id}", student);
});

// Обновление
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

// Удаление
app.MapDelete("/api/students/{id}", async (int id, AppDbContext db) =>
{
    var student = await db.Students.FirstOrDefaultAsync(s => s.Id == id);
    if (student == null) return Results.NotFound();

    db.Students.Remove(student);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");