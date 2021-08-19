using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UserApiSql.Models;

namespace UserControlUI.ModelsDTO
{
    public class Noti : BaseEntity<int>
    {
        [Required]
        public User FromUserId { get; set; }
        [Required]
        public User ToUserId { get; set; }
        [Required]
        [MaxLength(300)]
        public string NotiHeader { get; set; } = "";
        [Required]
        [MaxLength(300)]
        public string NotiBody { get; set; } = "";
        public bool IsRead { get; set; } = false;
        [Required]
        [MaxLength(100)]
        public string Url { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        [MaxLength(100)]
        public string Message { get; set; } = "";
    }
}
