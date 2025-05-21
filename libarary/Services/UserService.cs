using libarary.Data;
using libarary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace libarary.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // Register a new user
        public void RegisterUser(User user)
        {
            if (_context.Users.Any(u => u.NationalIdNumber == user.NationalIdNumber))
                throw new InvalidOperationException("User with this National ID already exists.");

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // Get a list of all users
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        // Delete a user by their ID
        public void DeleteUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            
            if (user == null)
                throw new InvalidOperationException("User not found.");

            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        // Update a user's information
        public void UpdateUser(int userId, User updatedUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            
            if (user == null)
                throw new InvalidOperationException("User not found.");

            user.Name = updatedUser.Name;
            user.Sex = updatedUser.Sex;
            user.NationalIdNumber = updatedUser.NationalIdNumber;
            user.Address = updatedUser.Address;

            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
