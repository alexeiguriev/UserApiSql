using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiSql.Interfaces
{
    public interface IRepository<TEntity,TInputData>
    {
        public TEntity Create(TInputData entity);

        public void Delete(int id);

        public IEnumerable<TEntity> Get();

        public TEntity Get(int id);

        public void Update(int id, TInputData entity);
    }
}
