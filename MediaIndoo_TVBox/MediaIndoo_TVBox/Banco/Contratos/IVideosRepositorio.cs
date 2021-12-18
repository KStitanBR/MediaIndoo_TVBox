using MediaIndoo_TVBox.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaIndoo_TVBox.Banco.Contratos
{
    public interface IVideosRepositorio : IBaseRepositorio<Videos>
    {
        List<Videos> GetByIdFather(int codigo);
        Videos GetByGuid(Guid guid);

    }
}
