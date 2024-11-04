using Logic.DAL.Entities;
using Logic.DAL.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Logic.Modules.UserModule;

public interface IUserRepository
{
    bool Any(RegisterRequest email);
    Task<UserEntity?> FindByPhoneAsync(string phoneNumber);
    Task<UserEntity?> FindByEmailAsync(string email);
    public Task<int> SaveChangesAsync();
    public Task<EntityEntry<UserEntity>> AddAsync(UserEntity userEntity);
    public Task<List<UserEntity>> ToListAsync();
    public void Remove(UserEntity userEntity);
    public EntityEntry<UserEntity> Update(UserEntity userEntity);
    public Task<UserEntity?> FindByPhoneNumberAsync(string phone);
}