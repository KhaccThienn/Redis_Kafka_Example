using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Remake_Kafka_Example_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService         _iproductService;
        private readonly IProductProducer        _productProducer;
        public ProductController(IProductService productService, IProductProducer productProducer)
        {
            _iproductService = productService;
            _productProducer = productProducer;
        }

        [HttpGet]
        public async Task<List<TableProduct_V2>> GetProducts()
        {
            return await _iproductService.GetProducts();
        }

        [HttpPost("insert-product")]
        public async Task<IActionResult> InsertProduct([FromBody] InsertProductRequest request)
        {
            await _productProducer.ProduceInsertProductAsync(request);
            return Ok(new { Status = "InsertProduct message sent to Kafka topic 'product-input'" });
        }

        [HttpPost("update-quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] UpdateQuantityRequest request)
        {
            await _productProducer.ProduceUpdateQuantityAsync(request);
            return Ok(new { Status = "UpdateQuantity message sent to Kafka topic 'product-input'" });
        }

        [HttpPost("update-price")]
        public async Task<IActionResult> UpdatePrice([FromBody] UpdatePriceRequest request)
        {
            await _productProducer.ProduceUpdatePriceAsync(request);
            return Ok(new { Status = "UpdatePrice message sent to Kafka topic 'product-input'" });
        }
    }
}
