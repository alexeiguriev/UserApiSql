using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Interfaces;
using UserApiSql.Models;

namespace UserApiSql.Data
{
    public class UserRepository : IRepository<User,User>
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task Delete(int id)
        {
            User userToDelete = await _context.Users.FirstOrDefaultAsync(i => i.Id == id);
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> Get()
        {
            IEnumerable<User> user = await _context.Users
                .Include(ur => ur.UserRoles)
                .ThenInclude(urr => urr.Role)
                .Include(d => d.Documents)
                .ToListAsync();

            return user;
        }

        public async Task<User> Get(int id)
        {
            User user = await _context.Users
                .Include(ur => ur.UserRoles)
                .ThenInclude(urr => urr.Role)
                .Include(d => d.Documents)
                .FirstOrDefaultAsync(i => i.Id == id);
            return user;
        }

        public async Task Update(int id, User user)
        {
            user.Id = id;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
