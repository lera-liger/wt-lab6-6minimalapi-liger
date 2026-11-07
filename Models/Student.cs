using System.ComponentModel.DataAnnotations;

namespace wt_lab6_6minimalapi_liger.Models;

public class Student
{
    public int Id { get; set; }

    [Required(ErrorMessage = "ФИО обязательно для заполнения")]
    [StringLength(100, MinimumLength = 5,
        ErrorMessage = "ФИО должно содержать от 5 до 100 символов")]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "Специальность обязательна")]
    [StringLength(100,
        ErrorMessage = "Название специальности не должно превышать 100 символов")]
    public string Specialty { get; set; } = "";

    [Range(1, 4, ErrorMessage = "Курс должен быть от 1 до 4")]
    public int Course { get; set; }

    [Range(0, 10, ErrorMessage = "Средний балл должен быть от 0 до 10")]
    public double AverageGrade { get; set; }

    public bool IsEnrolled { get; set; }
}
