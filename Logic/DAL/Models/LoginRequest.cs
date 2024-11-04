using System.ComponentModel.DataAnnotations;

namespace Logic.DAL.Models;

public class LoginRequest
{
    [Required]
    [RegularExpression(@"^(\+7|8)\d{10}$", ErrorMessage = "Введите корректный номер телефона.")]
    public string Phone { get; set; }
    
    [Required]
    [StringLength(20, MinimumLength = 8,  ErrorMessage = "Длина пароля: 8-20")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}