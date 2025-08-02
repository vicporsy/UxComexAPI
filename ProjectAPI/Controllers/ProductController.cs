using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vicporsy.Contract.Request;
using Vicporsy.Contract.Response;
using Vicporsy.Domain.Interface.Services;
using Vicporsy.Domain.Models;

namespace Vicporsy.ProjectAPI.Controllers
{
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        #region [Constructor]
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly HttpContext httpContext;
        public ProductController(IProductService productService, IMapper mapper, IHttpContextAccessor httpContextAcessor)
        {
            _productService = productService;
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

            var dados = await _productService.GetProductAsync(id);

            var response = _mapper.Map<ProductResponse>(dados);

            httpContext.Items.Add("Response", response);
            return Ok(new ApiResponse<ProductResponse>
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

            var dados = await _productService.GetAllProductAsync();

            var response = _mapper.Map<List<ProductResponse>>(dados);

            httpContext.Items.Add("Response", response.Count);
            return Ok(new ApiResponse<List<ProductResponse>>
            {
                success = true,
                data = response
            });
        }

        #endregion

        #region [POST]
        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] ProductRequest request)
        {
            try
            {
                httpContext.Items.Add("Request", request);

                var response = await _productService.PostProductAsync(request);

                httpContext.Items.Add("Response", response);
                return Ok(new ApiResponse<object>
                {
                    success = true,
                    message = "Product created successfully."
                });
            }
            catch(Exception ex)
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
        public async Task<IActionResult> Put([FromBody] ProductRequest request)
        {
            try
            {
                httpContext.Items.Add("Request", request);

                var response = await _productService.PutProductAsync(request);

                httpContext.Items.Add("Response", response);
                return Ok(new ApiResponse<object>
                {
                    success = true,
                    message = "Produto atualizado com sucesso"
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

                var response = await _productService.DeleteProductAsync(id);

                httpContext.Items.Add("Response", response);
                return Ok(new ApiResponse<object>
                {
                    success = true,
                    message = "Produto removido com sucesso"
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
