using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Interfaces;

namespace UserApiSql.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserContext dc;

        public UnitOfWork(UserContext dc)
        {
            this.dc = dc;
        }
        public IUserRepository UserRepository =>
            new UserRepository(dc);

        public async Task<bool> SaveAsync()
        {
            return await dc.SaveChangesAsync() > 0;
        }
    }
}
