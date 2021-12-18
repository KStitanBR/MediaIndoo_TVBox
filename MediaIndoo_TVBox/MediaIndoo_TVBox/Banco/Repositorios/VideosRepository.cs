using MediaIndoo_TVBox.Banco.Contratos;
using MediaIndoo_TVBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaIndoo_TVBox.Banco.Repositorios
{
    public class VideosRepository : BaseRepositorio<Videos>, IVideosRepositorio
    {
        public VideosRepository()
        {
        }

        public List<Videos> GetByIdFather(int codigo)
        {
            var result = DbContext.Videos.ToList()
                                                  .Where(u => u.PlayerID.Equals(codigo)).ToList();
            return result;
        }
        public Videos GetByGuid(Guid guid)
        {
            var result = DbContext.Videos.ToList()
                                                  .FirstOrDefault(u => u.VideoGuid.Equals(guid));
            return result;
        }
    }
}
