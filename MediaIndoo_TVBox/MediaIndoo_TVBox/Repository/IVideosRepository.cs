using MediaIndoo_TVBox.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaIndoo_TVBox.Repository
{
    public interface IVideosRepository
    {
        Task<ResponseService<List<Videos>>> GetAllVideos();
        Task<ResponseService<List<Videos>>> GetAllById(int playerId);
        Task<ResponseService<List<Videos>>> GetPartVideo(int codigo);
        Task<ResponseService<Videos>> GetByGuid(Guid guid);
    }
}
