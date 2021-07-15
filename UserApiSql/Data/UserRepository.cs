using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Interfaces;
using UserApiSql.Models;

namespace UserApiSql.Data
{
    public class UserRepository : IRepository<User, UserInput>
    {
        private readonly UserContext _context;
        private readonly IMapper _mapper;

        public UserRepository(UserContext context)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserInput, User>());
            _context = context;
            _mapper = new Mapper(config);
        }

        public async Task<User> Create(UserInput userInput)
        {
            User user = new User();
            user = _mapper.Map<User>(userInput);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            foreach (int userRoleId in userInput.UserRolesId)
            {
                UserRole userRole = new UserRole { UserId = user.Id, RoleId = userRoleId };

                _context.UserRoles.Add(userRole);
                _context.SaveChanges();
            }

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
        public async Task<User> Update(int id, UserInput userInput)
        {
            User user = new User();
            user = _mapper.Map<User>(userInput);
            user.Id = id;


            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            await UpdateUserRoles(id, userInput.UserRolesId);

            return user;
        }
        private async Task<User> GetUserAsNoTracking(int id)
        {
            User user = await _context.Users.AsNoTracking()
                .Include(ur => ur.UserRoles)
                .ThenInclude(urr => urr.Role)
                .Include(d => d.Documents)
                .FirstOrDefaultAsync(i => i.Id == id);
            return user;
        }
        private async Task UpdateUserRoles(int id, int[] newUserRolesId)
        {
            // Remove all ald user roles
            _context.UserRoles.RemoveRange(_context.UserRoles.Where(x => x.UserId == id));
            await _context.SaveChangesAsync();

            // Add all new user roles
            foreach (int userRoleId in newUserRolesId)
            {
                UserRole userRole = new UserRole { UserId = id, RoleId = userRoleId };

                await _context.UserRoles.AddAsync(userRole);
            }
            await _context.SaveChangesAsync();

        }

    }
}
