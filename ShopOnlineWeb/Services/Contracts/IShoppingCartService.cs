using ShopOnlineModels.Dto;

namespace ShopOnlineWeb.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<List<CartItemDto>> GetItems(int userId);
        Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<CartItemDto> DeleteItem(int id);
        Task<CartItemDto> UpdateQty(CartItemQtyUpdateDto cartItemQtyUpdate);

        event Action<int> OnShoppingCartChanged;
        void RaiseEventOnShoppingCartCharged(int totalQty);
    }
}
