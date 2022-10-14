using Microsoft.EntityFrameworkCore;
using ShopOnlineModels.Dto;
using ShoppingWebAPI.Data;
using ShoppingWebAPI.Entities;
using ShoppingWebAPI.Repositories.Contracts;

namespace ShoppingWebAPI.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        public ShopOnlineDbContext ShopOnlineDbContext { get; }
        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            ShopOnlineDbContext = shopOnlineDbContext;
        }

        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await this.ShopOnlineDbContext.CartItems.AnyAsync(c => c.CartId == cartId &&
                                                                     c.ProductId == productId);
        }

        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from product in this.ShopOnlineDbContext.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      ProductId = product.Id,
                                      CartId = cartItemToAddDto.CartId,
                                      Qty = cartItemToAddDto.Qty
                                  }).SingleOrDefaultAsync();
                if (item != null)
                {
                    var result = await this.ShopOnlineDbContext.CartItems.AddAsync(item);
                    await this.ShopOnlineDbContext.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;
        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await this.ShopOnlineDbContext.CartItems.FindAsync(id);
            if(item != null)
            {
                this.ShopOnlineDbContext.CartItems.Remove(item);
                await this.ShopOnlineDbContext.SaveChangesAsync();
            }
            return item;
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in this.ShopOnlineDbContext.Carts
                          join cartItem in this.ShopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id= cartItem.Id,
                              ProductId = cartItem.ProductId,
                              CartId= cartItem.CartId,
                              Qty= cartItem.Qty
                          }).SingleAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in this.ShopOnlineDbContext.Carts
                          join cartItem in this.ShopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              CartId = cartItem.CartId,
                              Qty = cartItem.Qty
                          }).ToListAsync();
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await this.ShopOnlineDbContext.CartItems.FindAsync(id);

            if(item != null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty;
                await this.ShopOnlineDbContext.SaveChangesAsync();
                return item;
            }

            return null;
        }
    }
}
