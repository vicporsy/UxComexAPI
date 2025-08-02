using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Vicporsy.Contract.Request;
using Vicporsy.Contract.Response;

namespace Vicporsy.Wrapper
{

    //Transformar em pacote nuget para acesso a chamadas da api (encapsulado)
    public class OrderWrapper
    {
        private string baseUrl = "https://localhost:44366/";
        public OrderWrapper()
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<ApiResponse<OrderResponse>> Get(int id)
        {
            var dados = await baseUrl
                .AppendPathSegment("Order")
                .AppendPathSegment("Get")
                .SetQueryParams(new { id = id })
                .GetJsonAsync<ApiResponse<OrderResponse>>();
            return dados;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ApiResponse<List<OrderResponse>>> GetAll()
        {
            var dados = await baseUrl
                .AppendPathSegment("Order")
                .AppendPathSegment("GetAll")
                .GetJsonAsync<ApiResponse<List<OrderResponse>>>();
            return dados;
        }

        [HttpPost]
        [Route("Post")]
        public async Task<ApiResponse<object>> Post([FromBody] OrderRequest request)
        {
            return await baseUrl
                .AppendPathSegment("Order")
                .AppendPathSegment("Post")
                .PostJsonAsync(request)
                .ReceiveJson<ApiResponse<object>>();
        }

        [HttpPut]
        [Route("Put")]
        public async Task<ApiResponse<object>> Put([FromBody] OrderRequest request)
        {
            return await baseUrl
                .AppendPathSegment("Order")
                .AppendPathSegment("Put")
                .PutJsonAsync(request)
                .ReceiveJson<ApiResponse<object>>();
        }
    }
}
