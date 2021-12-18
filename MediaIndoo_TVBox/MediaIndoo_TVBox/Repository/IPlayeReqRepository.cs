using MediaIndoo_TVBox.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaIndoo_TVBox.Repository
{
    public interface IPlayeReqRepository
    {
        Task<ResponseService<List<PlayeReq>>> GetAllPlayerReqs();
        Task<ResponseService<UltimoAcessoPlayer>> GetAllPlayeReqsId(int PlayerId);
        Task<ResponseService<PlayeReq>> PostAsync(PlayeReq playeReq);
    }
}
