using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiSql.ModelsDTO
{
    public class Redactor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class DocumentDTO
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
        public DateTime UploadedDate { get; set; }
        public string UpdatedBy { get; set; }
        public byte[] Content { get; set; }
    }
}
