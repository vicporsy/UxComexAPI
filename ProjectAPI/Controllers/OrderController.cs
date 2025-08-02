using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vicporsy.Contract.Request;
using Vicporsy.Contract.Response;
using Vicporsy.Domain.Interface.Services;

namespace Vicporsy.ProjectAPI.Controllers
{
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        #region [Constructor]
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly HttpContext httpContext;
        public OrderController(IOrderService orderService, IMapper mapper, IHttpContextAccessor httpContextAcessor)
        {
            _orderService = orderService;
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

            var dados = await _orderService.GetOrderAsync(id);

            var response = _mapper.Map<OrderResponse>(dados);

            httpContext.Items.Add("Response", response);
            return Ok(new ApiResponse<OrderResponse>
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

            var dados = await _orderService.GetAllOrderAsync();

            var response = _mapper.Map<List<OrderResponse>>(dados);

            httpContext.Items.Add("Response", response.Count);
            return Ok(new ApiResponse<List<OrderResponse>>
            {
                success = true,
                data = response
            });
        }

        #endregion

        #region [POST]
        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] OrderRequest request)
        {
            try
            {
                httpContext.Items.Add("Request", request);

                var response = await _orderService.PostOrderAsync(request);

                httpContext.Items.Add("Response", response);
                return Ok(new ApiResponse<object>
                {
                    success = true,
                    message = "Sucesso ao inserir o pedido!"
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse<object>
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
        public async Task<IActionResult> Put([FromBody] OrderRequest request)
        {
            try
            {
                httpContext.Items.Add("Request", request);

                var response = await _orderService.PutOrderAsync(request);

                httpContext.Items.Add("Response", response);
                return Ok(new ApiResponse<object>
                {
                    success = true,
                    message = "Sucesso ao atualizar o pedido!"
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        #endregion
    }
}
