using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mappings.DTO
{
    public class DataTableDTO<T>
    {
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
        public List<T> Data { get; set; }
    }
}
