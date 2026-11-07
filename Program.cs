using wt_lab6_6minimalapi_liger.Data;
using wt_lab6_6minimalapi_liger.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();  // OpenAPI

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "wt-lab6-6minimalapi-liger v1");
    });
}

app.MapGet("/", () => "Minimal API работает!");
app.MapGet("/api/info", () => new { Application = "Web Technologies", Technology = "ASP.NET Core Minimal API", Version = "1.0" });

app.MapGet("/api/students", (string? search) =>
{
    var students = StudentStore.Students.AsEnumerable();
    if (!string.IsNullOrWhiteSpace(search))
    {
        students = students.Where(s => s.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                       s.Specialty.Contains(search, StringComparison.OrdinalIgnoreCase));
    }
    return Results.Ok(students);
});

app.MapGet("/api/students/{id}", (int id) =>
{
    var student = StudentStore.Students.FirstOrDefault(s => s.Id == id);
    if (student == null) return Results.NotFound();
    return Results.Ok(student);
});

app.MapPost("/api/students", (Student student) =>
{
    if (string.IsNullOrWhiteSpace(student.FullName))
        return Results.BadRequest("ФИО обязательно для заполнения.");

    if (student.Course < 1 || student.Course > 4)
        return Results.BadRequest("Курс должен быть от 1 до 4.");

    if (student.AverageGrade < 0 || student.AverageGrade > 10)
        return Results.BadRequest("Средний балл должен быть от 0 до 10.");

    student.Id = StudentStore.Students.Any()
        ? StudentStore.Students.Max(s => s.Id) + 1
        : 1;

    StudentStore.Students.Add(student);

    return Results.Created($"/api/students/{student.Id}", student);
});

app.MapPut("/api/students/{id}", (int id, Student updatedStudent) =>
{
    var student = StudentStore.Students.FirstOrDefault(s => s.Id == id);
    if (student == null) return Results.NotFound();

    student.FullName = updatedStudent.FullName;
    student.Specialty = updatedStudent.Specialty;
    student.Course = updatedStudent.Course;
    student.AverageGrade = updatedStudent.AverageGrade;
    student.IsEnrolled = updatedStudent.IsEnrolled;

    return Results.Ok(student);
});

app.MapDelete("/api/students/{id}", (int id) =>
{
    var student = StudentStore.Students.FirstOrDefault(s => s.Id == id);
    if (student == null) return Results.NotFound();

    StudentStore.Students.Remove(student);
    return Results.NoContent();
});

app.Run();