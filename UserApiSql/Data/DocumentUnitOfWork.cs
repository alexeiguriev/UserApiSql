using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Interfaces;

namespace UserApiSql.Data
{
    public class DocumentUnitOfWork : IDocumentUnitOfWork
    {
        private readonly UserContext dc;

        public DocumentUnitOfWork(UserContext dc)
        {
            this.dc = dc;
        }
        public IDocumentRepository DocumentRepository =>
            new DocumentRepository(dc);

        public async Task<bool> SaveAsync()
        {
            return await dc.SaveChangesAsync() > 0;
        }
    }
}
