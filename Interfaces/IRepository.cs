using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiSql.Interfaces
{
    public interface IRepository<TEntity,TInputData>
    {
        public Task<TEntity> Create(TInputData entity);

        public Task Delete(int id);

        public Task<IEnumerable<TEntity>> Get();

        public Task<TEntity> Get(int id);

        public Task<TEntity> Update(int id, TInputData entity);
    }
}
