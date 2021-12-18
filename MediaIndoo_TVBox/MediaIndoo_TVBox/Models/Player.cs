using System;
using System.Collections.Generic;
using System.Text;

namespace MediaIndoo_TVBox.Models
{
    public class Player
    {
        public int Codigo { get; set; }
        public int UsuarioID { get; set; }
        public string Nome { get; set; }
        public Guid PlayerGuid { get; set; }

        public Player()
        {

        }
     
    }
}
