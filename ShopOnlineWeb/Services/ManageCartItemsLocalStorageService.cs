using Blazored.LocalStorage;
using ShopOnlineModels.Dto;
using ShopOnlineWeb.Services.Contracts;

namespace ShopOnlineWeb.Services
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {
        private ILocalStorageService localStorageService;
        private IShoppingCartService shoppingCartService;
        const string key = "CartItemCollection";

        public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService, 
                IShoppingCartService shoppingCartService)
        {
            this.localStorageService = localStorageService;
            this.shoppingCartService = shoppingCartService;
        }
        public async Task<List<CartItemDto>> GetCollection()
        {
            return await this.localStorageService.GetItemAsync<List<CartItemDto>>(key)
                        ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await this.localStorageService.RemoveItemAsync(key);
        }

        public async Task SaveCollection(List<CartItemDto> cartItems)
        {
            await this.localStorageService.SetItemAsync(key, cartItems);
        }

        private async Task<List<CartItemDto>> AddCollection()
        {
            var shoppingCartCollection = await this.shoppingCartService.GetItems(HardCorded.UserId);

            if(shoppingCartCollection == null)
            {
                await this.localStorageService.SetItemAsync(key, shoppingCartCollection);
            }
            return shoppingCartCollection;
        }
    }
}
