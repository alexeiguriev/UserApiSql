using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Data;
using UserApiSql.Models;

namespace UserApiSql.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            User userToDelete = _context.Users.FirstOrDefault(i => i.Id == id);
            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
        }

        public IEnumerable<User> Get()
        {
            IEnumerable<User> user = _context.Users.ToList();
            return user;
        }

        public User Get(int id)
        {
            User user = _context.Users.FirstOrDefault(i => i.Id == id);
            return user;
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
