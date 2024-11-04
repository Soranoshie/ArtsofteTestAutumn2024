using System.ComponentModel.DataAnnotations;
using Logic.Attributes;

namespace Logic.DAL.Entities;

public class UserEntity : IEntity<UserEntity>
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [FullName]
    [StringLength(250, ErrorMessage = "Превышена длина ФИО")]
    public string FIO { get; set; }
    
    [Required]
    [RegularExpression(@"^(\+7|8)\d{10}$", ErrorMessage = "Введите корректный номер телефона.")]
    public string Phone { get; set; }
    
    [Required]
    [EmailAddress]
    [StringLength(150, ErrorMessage = "Превышена длина почтового адреса")]
    public string Email { get; set; }
    
    [Required]
    [StringLength(20, MinimumLength = 8,  ErrorMessage = "Длина пароля: 8-20")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    public DateTime LastLogin { get; set; }
}