using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiSql.ModelsDTO
{
    public class InputDocument
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
        public int UpdaterId { get; set; }
        public byte[] Content { get; set; }
    }
}
