using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiSql.Models
{
    public class User : BaseEntity<int>
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        //public ICollection<UserRole> Roles { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<Document> Documents { get; set; }
    }
}
