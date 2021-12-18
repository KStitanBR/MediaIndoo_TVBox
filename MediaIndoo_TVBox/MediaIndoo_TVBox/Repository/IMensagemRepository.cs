using MediaIndoo_TVBox.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaIndoo_TVBox.Repository
{
   public interface IMensagemRepository
    {
        Task<ResponseService<List<Mensagem>>> GetAllMsgs();
        Task<ResponseService<List<Mensagem>>> GetAllMsgsById(int playerId);
    }
}
