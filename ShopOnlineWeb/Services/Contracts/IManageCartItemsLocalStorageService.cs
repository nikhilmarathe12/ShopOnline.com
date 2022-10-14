using ShopOnlineModels.Dto;

namespace ShopOnlineWeb.Services.Contracts
{
    public interface IManageCartItemsLocalStorageService
    {
        Task<List<CartItemDto>> GetCollection();
        Task SaveCollection(List<CartItemDto> cartItems);
        Task RemoveCollection();
    }
}
