using Microsoft.AspNetCore.Components;
using ShopOnlineModels.Dto;
using ShopOnlineWeb.Services.Contracts;

namespace ShopOnlineWeb.Pages
{
    public class ProductsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }   

        public IEnumerable<ProductDto> Products { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await ClearLocalStorage();

                //Products = await ProductService.GetItems();
                //var shoppingCartItems = await ShoppingCartService.GetItems(HardCorded.UserId);
                Products = await ManageProductsLocalStorageService.GetCollection();
                var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                var totalQty = shoppingCartItems.Sum(i => i.Qty);

                ShoppingCartService.RaiseEventOnShoppingCartCharged(totalQty);
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected IOrderedEnumerable<IGrouping<int,ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
                                            group product by product.CategoryId into prodByCatGroup
                                            orderby prodByCatGroup.Key
                                            select prodByCatGroup;
             
        }

        protected string GetCategoryName(IGrouping<int,ProductDto> groupedProductDto)
        {
            return groupedProductDto.FirstOrDefault(pg => pg.CategoryId == groupedProductDto.Key).CategoryName;
        }

        private async Task ClearLocalStorage()
        {
            await ManageProductsLocalStorageService.RemoveCollection();
            await ManageCartItemsLocalStorageService.RemoveCollection();
        }
    }
}
