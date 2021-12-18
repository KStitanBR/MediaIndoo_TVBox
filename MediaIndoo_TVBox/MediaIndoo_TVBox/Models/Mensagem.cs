using System;

namespace MediaIndoo_TVBox.Models
{
    public class Mensagem
    {
        public int Codigo { get; set; }
        public int UsuarioID { get; set; }
        public string Msg { get; set; }
        public Guid MensagemGuid { get; set; }
        public Mensagem()
        {

        }
        public Mensagem(int codigo, int usuarioID, string msg)
        {
            this.Codigo = codigo;
            this.UsuarioID = usuarioID;
            this.Msg = msg;
        }

    }
}
