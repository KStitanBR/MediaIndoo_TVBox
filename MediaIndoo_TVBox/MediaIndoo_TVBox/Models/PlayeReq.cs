using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaIndoo_TVBox.Models
{
    public class PlayeReq
    {
        public int Codigo { get; set; }
        public int PlayerID { get; set; }
        public DateTime data { get; set; }
        public Guid PlayeReqGuid { get; set; }
    }
}
