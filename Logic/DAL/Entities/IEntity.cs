using System.ComponentModel.DataAnnotations;

namespace Logic.DAL.Entities;

public interface IEntity<T>
{
    [Key]
    public Guid Id { get; set; }
}