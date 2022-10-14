using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnlineModels.Dto;
using ShopOnlineWeb.Services.Contracts;

namespace ShopOnlineWeb.Pages
{
    public class CheckoutBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }

        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }  

        protected int TotalQty { get; set; }

        protected string PaymentDescription { get; set; }

        protected decimal PaymentAmount { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                //ShoppingCartItems = await ShoppingCartService.GetItems(HardCorded.UserId);
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                if (ShoppingCartItems != null)
                {
                    Guid orderGuid = Guid.NewGuid();

                    PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                    TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                    PaymentDescription = $"O_{HardCorded.UserId}_{orderGuid}";
                }
            }
            catch (Exception)
            {
                //Log Exception
                throw;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {

                if(firstRender)
                {
                    await Js.InvokeVoidAsync("initPayPalButton");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
