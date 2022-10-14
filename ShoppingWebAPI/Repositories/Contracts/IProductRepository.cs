using ShoppingWebAPI.Data;
using ShoppingWebAPI.Entities;

namespace ShoppingWebAPI.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetItems();
        Task<IEnumerable<ProductCategory>> GetCategories();
        Task<Product> GetItem(int Id);
        Task<ProductCategory> GetCategory(int Id);
        Task<IEnumerable<Product>> GetItemsByCategory(int id);

    }
}
