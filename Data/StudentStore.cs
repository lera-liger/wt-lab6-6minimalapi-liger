using wt_lab6_6minimalapi_liger.Models;
namespace wt_lab6_6minimalapi_liger.Data;
public static class StudentStore
{
    public static List<Student> Students { get; } = new()
    {
        new Student
        {
            Id = 1,
            FullName = "Лигер Валерия Валерьевна",
            Specialty = "Управление информационными ресурсами",
            Course = 4,
            AverageGrade = 9.7,
            IsEnrolled = true
        },
        new Student
        {
            Id = 2,
            FullName = "Ясковец Александра Дмитриевна",
            Specialty = "Управление информационными ресурсами",
            Course = 4,
            AverageGrade = 10,
            IsEnrolled = true
        },
        new Student
        {
            Id = 3,
            FullName = "Сидоров Максим Андреевич",
            Specialty = "Государственное управление и экономика",
            Course = 4,
            AverageGrade = 7.8,
            IsEnrolled = true
        },
        new Student
        {
            Id = 4,
            FullName = "Козлова Мария Дмитриевна",
            Specialty = "Государственное управление и экономика",
            Course = 1,
            AverageGrade = 9.5,
            IsEnrolled = true
        },
        new Student
        {
            Id = 5,
            FullName = "Смирнов Алексей Павлович",
            Specialty = "Государственное управление и право",
            Course = 3,
            AverageGrade = 6.9,
            IsEnrolled = false
        },
        new Student
        {
            Id = 6,
            FullName = "Васильева Елена Викторовна",
            Specialty = "Государственное управление и право",
            Course = 2,
            AverageGrade = 8.9,
            IsEnrolled = true
        }
    };
}
