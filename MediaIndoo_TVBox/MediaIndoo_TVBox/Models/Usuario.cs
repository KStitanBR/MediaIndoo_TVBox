using System;
using System.Collections.Generic;
using System.Text;

namespace MediaIndoo_TVBox.Models
{
    public class Usuario
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public Guid UsuarioGuid { get; set; }
        public Usuario()
        {

        }
        public Usuario(int codigo, string nome, string email, string senha)
        {
            this.Codigo = codigo;
            this.Nome = nome;
            this.Email = email;
            this.Senha = senha;
        }
    }
}
