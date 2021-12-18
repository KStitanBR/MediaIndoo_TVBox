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
    public class PlayeReqService : BaseService, IPlayeReqRepository
    {
        public async Task<ResponseService<UltimoAcessoPlayer>> GetAllPlayeReqsId(int PlayerId)
        {
            var Uri = $"{BaseApiUrl}/PlayeReq/{PlayerId}";
            ResponseService<UltimoAcessoPlayer> responseService = new ResponseService<UltimoAcessoPlayer>();
            HttpResponseMessage response = await _Http.GetAsync(Uri);


            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {

                var Data = await response.Content.ReadAsStringAsync();
                var PersonJson = JsonConvert.DeserializeObject<UltimoAcessoPlayer>(Data);
                responseService.Data = PersonJson;

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

        public async Task<ResponseService<List<PlayeReq>>> GetAllPlayerReqs()
        {
            var Uri = $"{BaseApiUrl}/PlayeReq";
            ResponseService<List<PlayeReq>> responseService = new ResponseService<List<PlayeReq>>();
            HttpResponseMessage response = await _Http.GetAsync(Uri);


            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
            {

                var Data = await response.Content.ReadAsStringAsync();
                var PersonJson = JsonConvert.DeserializeObject<List<PlayeReq>>(Data);
                responseService.Data = PersonJson;

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

        public async Task<ResponseService<PlayeReq>> PostAsync(PlayeReq playeReq)
        {
            var json = JsonConvert.SerializeObject(playeReq);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+
            HttpResponseMessage response = await  _Http.PostAsync($"{BaseApiUrl}/PlayeReq", stringContent);
            ResponseService<PlayeReq> responseService = new ResponseService<PlayeReq>();
            responseService.IsSuccess = response.IsSuccessStatusCode;
            responseService.StatusCode = (int)response.StatusCode;


            if (response.IsSuccessStatusCode)
            {
                var Data = await response.Content.ReadAsStringAsync();
                var Json = JsonConvert.DeserializeObject<PlayeReq>(Data);
                responseService.Data = Json;
            }
            else
            {
                var problemResponse = response.Content.ReadAsStringAsync();

                //ResponseService<Person> problemResponse = response.Content.ReadAsStringAsync();
                //responseService.Errors = problemResponse.Errors;
            }
            return responseService;
        }
    }
}
