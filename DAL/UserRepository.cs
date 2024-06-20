using Entities.DTOs;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersRepo();

        Task<User> GetUserById(int id);

        Task<string> AddUser(User user);

        Task<User> Login(string userName, string password);

        Task<User> UpdateUser(int id, UserDTO updatedUser);

        Task<string> DeleteUser(int id);

        Task<string> BecomeHost(int id);

        Task<List<User>> GetHosts();
    }
    public class UserRepository : IUserRepository
    {
        EventPlannerContext _context = new EventPlannerContext();


        public async Task<List<User>> GetUsersRepo()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<string> AddUser(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                return "Username already exists.";

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return "Email already exists.";
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return "User added";
        }

        public async Task<User> Login(string userName, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == userName && u.Password == password);
            if (user == null)
                return null;
            return user;
        }

        public async Task<User> UpdateUser(int id, UserDTO updatedUser)
        {

            var user = await GetUserById(id);
            if (updatedUser.Name != "string") user.Name = updatedUser.Name;
            if (updatedUser.Username != "string") user.Username = updatedUser.Username;
            if (updatedUser.Email != "string") user.Email = updatedUser.Email;
            if (updatedUser.Phone != "string") user.Phone = updatedUser.Phone;
            if (updatedUser.Password != "string") user.Password = updatedUser.Password;
            if (updatedUser.Role != "string") user.Role = updatedUser.Role;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<string> DeleteUser(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserId == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return "User Deleted";
        }

        public async Task<string> BecomeHost(int id)
        {
            var user = await GetUserById(id);
            user.Role = "host";
            return "User has been updated";
        }

        public async Task<List<User>> GetHosts()
        {
            return await _context.Users.Where(h => h.Role == "host").ToListAsync();
        }
    }
}
