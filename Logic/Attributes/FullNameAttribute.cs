using System.ComponentModel.DataAnnotations;

namespace Logic.Attributes;

public class FullNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        if (value is string fio && !string.IsNullOrWhiteSpace(fio))
        {
            var parts = fio.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length is >= 2 and <= 6)
                return ValidationResult.Success;
        }

        return new ValidationResult(
            "Поле ФИО должно содержать минимум два слова: Фамилия и Имя, разделённые пробелом.");
    }
}