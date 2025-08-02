using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Vicporsy.Contract.Request;
using Vicporsy.Contract.Response;

namespace Vicporsy.Wrapper
{

    //Transformar em pacote nuget para acesso a chamadas da api (encapsulado)
    public class ClientWrapper
    {
        private string baseUrl = "https://localhost:44366/";
        public ClientWrapper()
        {
            
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ApiResponse<ClientResponse>> Get(int id)
        {
            var dados = await baseUrl
                .AppendPathSegment("Client")
                .AppendPathSegment("Get")
                .SetQueryParams(new {id = id})
                .GetJsonAsync<ApiResponse<ClientResponse>>();
            return dados;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ApiResponse<List<ClientResponse>>> GetAll()
        {
            var dados = await baseUrl
                .AppendPathSegment("Client")
                .AppendPathSegment("GetAll")
                .GetJsonAsync<ApiResponse<List<ClientResponse>>>();
            return dados;
        }

        [HttpPost]
        [Route("Post")]
        public async Task<ApiResponse<object>> Post([FromBody] ClientRequest request)
        {       
            return await baseUrl
                .AppendPathSegment("Client")
                .AppendPathSegment("Post")
                .PostJsonAsync(request)
                .ReceiveJson<ApiResponse<object>>();
        }

        [HttpPut]
        [Route("Put")]
        public async Task<ApiResponse<object>> Put([FromBody] ClientRequest request)
        {
            return await baseUrl
                .AppendPathSegment("Client")
                .AppendPathSegment("Put")
                .PutJsonAsync(request)
                .ReceiveJson<ApiResponse<object>>();
        }

        [HttpDelete]
        [Route($"Delete")]
        public async Task<ApiResponse<object>> Delete([FromQuery] int id)
        {
            return await baseUrl
                .AppendPathSegment("Client")
                .AppendPathSegment("Delete")
                .SetQueryParams(new {id = id})
                .DeleteAsync()
                .ReceiveJson<ApiResponse<object>>();
        }
    } 
}
