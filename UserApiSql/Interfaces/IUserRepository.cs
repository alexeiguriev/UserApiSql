using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiSql.Interfaces
{
    public interface IUserRepository<TEntity, TInputData> : IRepository<TEntity, TInputData>
    {
        public Task<TEntity> GetByEmail(string email);
        public Task<TEntity> GetByRoles(string Role);
    }
}
