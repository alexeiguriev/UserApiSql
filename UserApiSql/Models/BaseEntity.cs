using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiSql.Models
{
    public class BaseEntity<TKey>
    {
        [Column(Order = 0)]
        public TKey Id { get; set; }
    }
}
