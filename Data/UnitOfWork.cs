using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Interfaces;
using UserApiSql.Models;
using UserApiSql.ModelsDTO;

namespace UserApiSql.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserContext dc;

        public UnitOfWork(UserContext dc)
        {
            this.dc = dc;
        }
        public IUserRepository<User, UserInput> UserRepository =>
            new UserRepository(dc);
        public IRepository<Document, InputDocument> DocumentRepository =>
            new DocumentRepository(dc);
        public IRepository<Role, Role> RoleRepository =>
            new RoleRepository(dc);
        public IRepository<Noti, NotiInput> NotiRepository =>
            new NotiRepository(dc);

        public async Task<bool> SaveAsync()
        {
            return await dc.SaveChangesAsync() > 0;
        }
    }
}
