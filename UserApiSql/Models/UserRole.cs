using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiSql.Models
{
    public class UserRole : BaseEntity<int>
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
