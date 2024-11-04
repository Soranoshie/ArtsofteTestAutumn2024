using Logic.DAL;
using Logic.DAL.Entities;
using Logic.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Logic.Modules.UserModule;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext context;
    
    public UserRepository(ApplicationDbContext context)
    {
        this.context = context;
    }
    
    public bool Any(RegisterRequest model)
        => context.Users.Any(u => u.Email == model.Email || u.Phone == model.Phone);

    public async Task<UserEntity?> FindByPhoneAsync(string phoneNumber)
        => await context.Users.FindAsync(phoneNumber);

    public async Task<UserEntity?> FindByEmailAsync(string email)
    => await context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<int> SaveChangesAsync()
        => await context.SaveChangesAsync();
    
    public async Task<EntityEntry<UserEntity>> AddAsync(UserEntity? email)
        => await context.Users.AddAsync(email);

    public async Task<List<UserEntity>> ToListAsync()
        => await context.Users.ToListAsync();

    public void Remove(UserEntity? userEntity)
        => context.Users.Remove(userEntity);

    public EntityEntry<UserEntity> Update(UserEntity? userEntity)
        => context.Users.Update(userEntity);

    public async Task<UserEntity?> FindByPhoneNumberAsync(string phone)
        => await context.Users.FirstOrDefaultAsync(u => u.Phone == phone);
}