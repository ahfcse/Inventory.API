using Inventory.API.Data;
using Inventory.API.Interfaces;
using Inventory.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Where(u => u.Username == username && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            // In a real application, you would hash the password and compare hashes
            var user = await GetByUsernameAsync(username);

            if (user == null)
                return false;

            // Temporary: For demo purposes only (use password hashing in production)
            // Compare with hashed password in real applications
            return user.PasswordHash == password;
        }

        public async Task<User> AddUserAsync(User user)
        {
            // Check for existing username
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                throw new Exception("Username already exists");

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.IsDeleted = true;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string newPasswordHash)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.PasswordHash = newPasswordHash;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.UserId == userId && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return  await _context.Users.ToListAsync();
        }


        Task<bool> IUserRepository.ValidateCredentialsAsync(string username, string password)
        {
            throw new NotImplementedException();
        }


    }
}
