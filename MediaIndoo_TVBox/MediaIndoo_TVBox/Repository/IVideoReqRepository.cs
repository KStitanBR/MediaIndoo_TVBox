using MediaIndoo_TVBox.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaIndoo_TVBox.Repository
{
    public interface IVideoReqRepository
    {
        Task<ResponseService<List<VideoReq>>> GetAllVideoReqs();
        Task<ResponseService<VideoReq>> GetAllVideoReqsId(int PlayerId);
        ResponseService<string> PostAsync(VideoReq playeReq);
    }
}
