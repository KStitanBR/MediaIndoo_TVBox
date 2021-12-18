using MediaIndoo_TVBox.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaIndoo_TVBox.Repository
{
   public interface IPlayerRepository
    {
        Task<ResponseService<List<Player>>> GetAllPlayers();
        Task<ResponseService<List<Player>>> GetAllPlayersId(int usuarioID);
    }
}
