using System;
using System.Collections.Generic;
using System.Text;

namespace MediaIndoo_TVBox.Models
{
    public class Videos
    {
        public int Codigo { get; set; }
        public int PlayerID { get; set; }
        public string Nome { get; set; }
        public Byte[] UriByte { get; set; }
        //public string UriBase64 { get; set; }
        public DateTime Date { get; set; }
        public Guid VideoGuid { get; set; }
        public Videos()
        {

        }
    }
}
