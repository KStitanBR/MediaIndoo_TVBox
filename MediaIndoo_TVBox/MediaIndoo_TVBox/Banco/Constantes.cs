using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MediaIndoo_TVBox.Banco
{
    public static class Constantes
    {
        public const string NomeDoArquivo = "MediaIndooTvBox1.db3";

        public static string CaminhoDoBanco
        {
            get
            {
                var caminhoBase = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                return Path.Combine(caminhoBase, NomeDoArquivo);
            }
        }
    }
}
