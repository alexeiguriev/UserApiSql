using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Models;

namespace UserApiSql.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
