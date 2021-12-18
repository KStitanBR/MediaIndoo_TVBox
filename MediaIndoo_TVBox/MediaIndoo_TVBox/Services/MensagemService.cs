using MediaIndoo_TVBox.Models;
using MediaIndoo_TVBox.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MediaIndoo_TVBox.Services
{
    public class MensagemService : BaseService, IMensagemRepository
    {
        public async  Task<ResponseService<List<Mensagem>>> GetAllMsgs()
        {
            var Uri = $"{BaseApiUrl}/Mensagem";
            ResponseService<List<Mensagem>> responseService = new ResponseService<List<Mensagem>>();
            HttpResponseMessage response = await _Http.GetAsync(Uri);


            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {

                var Data = await response.Content.ReadAsStringAsync();
                var MsgJson = JsonConvert.DeserializeObject<List<Mensagem>>(Data);
                responseService.Data = MsgJson;

            }
            else
            {
                var problemResponse = response.Content.ReadAsStringAsync();
                //var errors = JsonConvert.DeserializeObject<ResponseService<List<Routine>>>(problemResponse);


                //responseService.Errors = problemResponse;
            }

            //Barrel.Current.Add(key: Uri, data: responseService.Data, expireIn: TimeSpan.FromMinutes(30));
            return responseService;
        }

        public async Task<ResponseService<List<Mensagem>>> GetAllMsgsById(int playerId)
        {
            var Uri = $"{BaseApiUrl}/Mensagem/{playerId}";
            ResponseService<List<Mensagem>> responseService = new ResponseService<List<Mensagem>>();
            HttpResponseMessage response = await _Http.GetAsync(Uri);


            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                var Data = await response.Content.ReadAsStringAsync();
                var MsgJson = JsonConvert.DeserializeObject<List<Mensagem>>(Data);
                responseService.Data = MsgJson;
            }
            else
            {
                var problemResponse = response.Content.ReadAsStringAsync();
                //var errors = JsonConvert.DeserializeObject<ResponseService<List<Routine>>>(problemResponse);


                //responseService.Errors = problemResponse;
            }

            //Barrel.Current.Add(key: Uri, data: responseService.Data, expireIn: TimeSpan.FromMinutes(30));
            return responseService;
        }
    }
}
