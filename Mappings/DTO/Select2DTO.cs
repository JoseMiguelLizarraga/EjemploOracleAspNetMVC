using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mappings.DTO
{
    public class Select2DTO
    {
        public int Total { get; set; }
        public List<Select2Detalle> Results { get; set; }
    } 

}
