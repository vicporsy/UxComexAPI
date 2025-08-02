using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vicporsy.Contract.Request;
using Vicporsy.Contract.Response;
using Vicporsy.Domain.Interface.Services;

namespace Vicporsy.ProjectAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        #region [Constructor]
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        private readonly HttpContext httpContext;
        public ClientController(IClientService clientService, IMapper mapper, IHttpContextAccessor httpContextAcessor)
        {
            _clientService = clientService;
            _mapper = mapper;
            this.httpContext = httpContextAcessor.HttpContext!;
        }

        #endregion

        #region [GET]

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            httpContext.Items.Add("Request", id);

            var dados = await _clientService.GetClientAsync(id);

            var response = _mapper.Map<ClientResponse>(dados);

            httpContext.Items.Add("Response", response);
            return Ok(new ApiResponse<ClientResponse>
            {
                success = true,
                data = response
            });
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            httpContext.Items.Add("Request", "");

            var dados = await _clientService.GetAllClientAsync();

            var response = _mapper.Map<List<ClientResponse>>(dados);

            httpContext.Items.Add("Response", response.Count);
            return Ok(new ApiResponse<List<ClientResponse>>
            {
                success = true,
                data = response
            });
        }

        #endregion

        #region [POST]
        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] ClientRequest request)
        {
            try
            {
                httpContext.Items.Add("Request", request);

                var response = await _clientService.PostClientAsync(request);

                httpContext.Items.Add("Response", response);
                return Ok(new ApiResponse<object>
                {
                    success = true,
                    message = "Sucesso ao inserir o cliente!"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse<object>
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        #endregion


        #region [PUT]
        [HttpPut]
        [Route("Put")]
        public async Task<IActionResult> Put([FromBody] ClientRequest request)
        {
            try
            {
                httpContext.Items.Add("Request", request);

                var response = await _clientService.PutClientAsync(request);

                httpContext.Items.Add("Response", response);
                return Ok(new ApiResponse<object>
                {
                    success = true,
                    message = "Sucesso ao atualizar o cliente!"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse<object>
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        #endregion

        #region [DELETE]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                httpContext.Items.Add("Request", id);

                await _clientService.DeleteClientAsync(id);

                httpContext.Items.Add("Response", "");
                return Ok(new ApiResponse<object>
                {
                    success = true,
                    message = "Sucesso ao apagar o cliente!"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse<object>
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        #endregion
    }
}
