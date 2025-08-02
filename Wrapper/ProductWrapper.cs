using Flurl;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using Vicporsy.Contract.Request;
using Vicporsy.Contract.Response;

namespace Vicporsy.Wrapper
{

    //Transformar em pacote nuget para acesso a chamadas da api (encapsulado)
    public class ProductWrapper
    {
        private string baseUrl = "https://localhost:44366/";
        public ProductWrapper()
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<ApiResponse<ProductResponse>> Get(int id)
        {
            var dados = await baseUrl
                .AppendPathSegment("Product")
                .AppendPathSegment("Get")
                .SetQueryParams(new { id = id })
                .GetJsonAsync<ApiResponse<ProductResponse>>();
            return dados;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ApiResponse<List<ProductResponse>>> GetAll()
        {
            var dados = await baseUrl
                .AppendPathSegment("Product")
                .AppendPathSegment("GetAll")
                .GetJsonAsync<ApiResponse<List<ProductResponse>>>();
            return dados;
        }

        [HttpPost]
        [Route("Post")]
        public async Task<ApiResponse<object>> Post([FromBody] ProductRequest request)
        {
            return await baseUrl
                .AppendPathSegment("Product")
                .AppendPathSegment("Post")
                .PostJsonAsync(request)
                .ReceiveJson<ApiResponse<object>>();
        }

        [HttpPut]
        [Route("Put")]
        public async Task<ApiResponse<object>> Put([FromBody] ProductRequest request)
        {
            return await baseUrl
                .AppendPathSegment("Product")
                .AppendPathSegment("Put")
                .PutJsonAsync(request)
                .ReceiveJson<ApiResponse<object>>();
        }

        [HttpDelete]
        [Route($"Delete")]
        public async Task<ApiResponse<object>> Delete([FromQuery] int id)
        {
            return await baseUrl
                .AppendPathSegment("Product")
                .AppendPathSegment("Delete")
                .SetQueryParams(new { id = id })
                .DeleteAsync()
                .ReceiveJson<ApiResponse<object>>();
        }
    }
}
