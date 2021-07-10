using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiSql.Interfaces
{
    public interface IDocumentUnitOfWork
    {
        IDocumentRepository DocumentRepository { get; }
        Task<bool> SaveAsync();
    }
}
