using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Models;
using UserApiSql.ModelsDTO;

namespace UserApiSql.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository<User, UserInput> UserRepository { get; }
        IRepository<Document, InputDocument> DocumentRepository { get; }
        IRepository<Role, Role> RoleRepository { get; }
        Task<bool> SaveAsync();
    }
}
