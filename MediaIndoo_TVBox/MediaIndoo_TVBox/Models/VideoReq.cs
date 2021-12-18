using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MediaIndoo_TVBox.Models
{
    public class VideoReq
    {
        public int Codigo { get; set; }
        public int PlayerID { get; set; }
        [ForeignKey(nameof(PlayerID))]
        public virtual Player Player { get; set; }
        public string NomeVideo { get; set; }
        public DateTime Date { get; set; }
        public Guid Guid { get; set; }
    }
}
