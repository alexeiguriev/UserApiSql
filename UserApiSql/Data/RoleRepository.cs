using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Interfaces;
using UserApiSql.Models;

namespace UserApiSql.Data
{
    public class RoleRepository : IRepository<Role, Role>
    {
        private readonly UserContext _context;

        public RoleRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<Role> Create(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            return role;
        }

        public async Task Delete(int id)
        {
            Role roleToDelete = await _context.Roles.FirstOrDefaultAsync(i => i.Id == id);
            _context.Roles.Remove(roleToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Role>> Get()
        {
            IEnumerable<Role> role = await _context.Roles
                .Include(ur => ur.UserRoles)
                .ThenInclude(uru => uru.User)
                .ToListAsync();

            return role;
        }

        public async Task<Role> Get(int id)
        {
            Role role = await _context.Roles
                .Include(ur => ur.UserRoles)
                .FirstOrDefaultAsync(i => i.Id == id);
            return role;
        }
        public async Task<Role> GetByRoleName(string roleName)
        {
            Role role = await _context.Roles
                .Include(ur => ur.UserRoles)
                .FirstOrDefaultAsync(i => i.Name == roleName);
            return role;
        }

        public async Task<Role> Update(int id, Role role)
        {
            role.Id = id;
            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return role;
        }
    }
}
