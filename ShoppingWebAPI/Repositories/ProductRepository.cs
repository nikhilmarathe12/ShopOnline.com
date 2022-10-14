using Microsoft.EntityFrameworkCore;
using ShoppingWebAPI.Data;
using ShoppingWebAPI.Entities;
using ShoppingWebAPI.Repositories.Contracts;

namespace ShoppingWebAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public ShopOnlineDbContext ShopOnlineDbContext { get; }
        public ProductRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            ShopOnlineDbContext = shopOnlineDbContext;
        }

        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await ShopOnlineDbContext.ProductCategories.ToListAsync();
            return categories;
        }

        public async Task<ProductCategory> GetCategory(int Id)
        {
            var category = await ShopOnlineDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == Id);
            return category;
        }

        public async Task<IEnumerable<Product>> GetItems()
        {
            var products = await ShopOnlineDbContext.Products
                                            .Include(p => p.ProductCategory)
                                            .ToArrayAsync();
            return products;
        }

        public async Task<Product> GetItem(int Id)
        {
            var Product = await ShopOnlineDbContext.Products
                                        .Include(p => p.ProductCategory)
                                        .SingleOrDefaultAsync( p => p.Id == Id);
            return Product;
        }

        public async Task<IEnumerable<Product>> GetItemsByCategory(int id)
        {
            var products = await ShopOnlineDbContext.Products
                                            .Include(p => p.ProductCategory)
                                            .Where( p => p.CategoryId == id).ToArrayAsync();
            return products;
        }
    }
}
