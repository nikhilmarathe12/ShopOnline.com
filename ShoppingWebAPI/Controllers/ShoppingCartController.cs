using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineModels.Dto;
using ShoppingWebAPI.Extension;
using ShoppingWebAPI.Repositories.Contracts;
using System.Formats.Asn1;

namespace ShoppingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        public IShoppingCartRepository ShoppingCartRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, 
                                      IProductRepository productRepository)
        {
            ShoppingCartRepository = shoppingCartRepository;
            ProductRepository = productRepository;
        }

        [HttpGet]
        [Route("{userId}/GetItems")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
        {
            try
            {
                var cartItems = await ShoppingCartRepository.GetItems(userId);

                if (cartItems == null)
                {
                    return NoContent();
                }
                var products = await this.ProductRepository.GetItems();
                if (products == null)
                {
                    throw new Exception("No products exist in the system");
                }
                var cartItemDto = cartItems.ConvetToDto(products);
                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<CartItemDto>> GetItem(int Id)
        {
            try
            {
                var cartItem = await this.ShoppingCartRepository.GetItem(Id);
                if (cartItem == null)
                {
                    return NoContent();
                }

                var product = await this.ProductRepository.GetItem(cartItem.ProductId);
                if (product == null)
                {
                    return NoContent();
                }
                var cartItemDto = cartItem.ConvertToDto(product);
                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemToAddDto cartItemToAddDto) 
        {
            try
            {
                var newCartItem = await this.ShoppingCartRepository.AddItem(cartItemToAddDto);
                if (newCartItem == null)
                {
                    return NoContent();
                }

                var product = await ProductRepository.GetItem(newCartItem.ProductId);

                if (product == null)
                {
                    throw new Exception($"Something went wrong when attempting to return product (productId:({cartItemToAddDto.ProductId})");
                }

                var cartItemDto = newCartItem.ConvertToDto(product);

                return CreatedAtAction(nameof(GetItem), new { id = newCartItem.Id }, cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem(int id)
        {
            try
            {
                var cartItem = await this.ShoppingCartRepository.DeleteItem(id);
                if (cartItem == null)
                {
                    return NoContent();
                }

                var product = await this.ProductRepository.GetItem(cartItem.ProductId);

                if (product == null)
                    return NoContent();

                var cartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartItemDto>> updateQty(int id, CartItemQtyUpdateDto  cartItemQtyUpdateDto )
        {
            try
            {
                var cartItem = await this.ShoppingCartRepository.UpdateQty(id, cartItemQtyUpdateDto);
                if(cartItem == null)
                {
                    return NoContent();
                }

                var product = await ProductRepository.GetItem(cartItem.ProductId);
                var cartItemDto = cartItem.ConvertToDto(product);
                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    } 
}
