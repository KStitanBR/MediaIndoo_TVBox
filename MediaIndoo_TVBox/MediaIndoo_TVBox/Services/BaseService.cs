using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MediaIndoo_TVBox.Services
{
    public class BaseService
    {
        protected HttpClient _Http;
        protected string BaseApiUrl = @"http://191.252.64.6/mi/api";
        //protected string BaseApiUrl = @"http://192.168.18.99:5000/api";
        //protected string BaseApiUrl = @"http://192.168.18.79:5000/api";

        public BaseService()
        {
            _Http = new HttpClient();

        }
    }
}
