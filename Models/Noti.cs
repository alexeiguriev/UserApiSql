using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiSql.Models
{
    public class Noti : BaseEntity<int>
    {
        [Required]
        public User FromUser { get; set; }
        [Required]
        public User ToUser { get; set; }
        [Required]
        [MaxLength(300)]
        public string NotiHeader { get; set; } = "";
        [Required]
        [MaxLength(300)]
        public string NotiBody { get; set; } = "";
        [Required]
        public bool IsRead { get; set; } = false;
        [Required]
        [MaxLength(100)]
        public string Url { get; set; } = "";
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        [MaxLength(100)]
        public string Message { get; set; } = "";
    }
}
