using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiSql.Models
{
    public class RoleDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string[] ReachedByUsers { get; set; }
    }
}
