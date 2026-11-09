namespace wt_lab7_7postgres_efcore_liger.Models;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public int Course { get; set; }
    public double AverageGrade { get; set; }
    public bool IsEnrolled { get; set; }
    public DateTime CreatedAt { get; set; }
}