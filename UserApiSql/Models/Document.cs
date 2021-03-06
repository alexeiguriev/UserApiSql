using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiSql.Models
{
    public class Document : BaseEntity<int>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Type { get; set; }
        public int Status { get; set; }
        public DateTime UploadedDate { get; set; }
        [Required]
        public User UpdatedBy { get; set; }
        [MaxLength(10000000)]
        public byte[] Content { get; set; }
    }
}
