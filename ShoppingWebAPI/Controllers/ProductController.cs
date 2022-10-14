using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineModels.Dto;
using ShoppingWebAPI.Extension;
using ShoppingWebAPI.Repositories.Contracts;
using ShoppingWebAPI.Entities;

namespace ShoppingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public IProductRepository ProductRepository { get; }
        public ProductController(IProductRepository productRepository)
        {
            ProductRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var product = await ProductRepository.GetItems();
                if(product == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = product.ConvertToDto();
                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriveing data from database");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int Id)
        {
            try
            {
                var product = await ProductRepository.GetItem(Id);
                if (product == null )
                {
                    return BadRequest();
                }
                else
                {
                    var productDtos = product.ConvertToDto();
                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriveing data from database");
            }
        }


        [HttpGet]
        [Route(nameof(GetProductCategories))]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetProductCategories()
        {
            try
            {
                var productCategories = await ProductRepository.GetCategories();

                var productCategoryDtos = productCategories.ConvertToDto();

                return Ok(productCategoryDtos);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                "Error retrieving data from the database");
            }

        }


        [HttpGet]
        [Route("{categoryId}/GetItemsByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
        {
            try
            {
                var products = await ProductRepository.GetItemsByCategory(categoryId);
                var productDtos = products.ConvertToDto();

                return Ok(productDtos);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                               "Error retrieving data from the database");
            }
        }
    }
}
