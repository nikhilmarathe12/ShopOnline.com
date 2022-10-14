using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnlineModels.Dto;
using ShopOnlineWeb.Services.Contracts;
using System.Formats.Asn1;

namespace ShopOnlineWeb.Pages
{
    public class ShoppingCartDetailBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public IShoppingCartService shoppingCartService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }
        public List<CartItemDto> shoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }
        public string TotalPrice { get; set; }
        public int TotalQuantity { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                //shoppingCartItems = await shoppingCartService.GetItems(HardCorded.UserId);
                shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                CartChanged();
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await shoppingCartService.DeleteItem(id);
            RemoveCartItem(id);
            CartChanged();
        }

        private CartItemDto GetCartItemDto(int id)
        {
            return shoppingCartItems.FirstOrDefault(i => i.Id == id);
        }

        private async Task RemoveCartItem(int id)
        {
            var cartItemDto = GetCartItemDto(id);

            shoppingCartItems.Remove(cartItemDto); 
            await ManageCartItemsLocalStorageService.SaveCollection(shoppingCartItems);
        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if(qty >0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto { CartItemId = id, Qty= qty };
                    var retunedUpdateItemDto = await this.shoppingCartService.UpdateQty(updateItemDto);
                    await UpdateItemTotalPrice(retunedUpdateItemDto);
                    CartChanged();

                    await Js.InvokeVoidAsync("MakeupdateQtyButtonVisible", id, false);
                }
                else
                {
                    var item = this.shoppingCartItems.FirstOrDefault(i => i.Id == id);
                    if(item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItemDto(cartItemDto.Id);

            if(item != null)
            {
                item.TotalPrice = cartItemDto.TotalPrice * cartItemDto.Qty;
            }
            await ManageCartItemsLocalStorageService.SaveCollection(shoppingCartItems);
        }

        private void CalculateCartSummaryTotals()
        {
            SetTotalPriceToQty();
            SetTotalPrice();
           
        }
        private void SetTotalPrice()
        {
            TotalPrice = this.shoppingCartItems.Sum( p => p.TotalPrice).ToString("C");
        }
        private void SetTotalQuantity()
        {
            TotalQuantity = this.shoppingCartItems.Sum(p => p.Qty);
        }
        private void SetTotalPriceToQty()
        {
            foreach (var item in this.shoppingCartItems)
            {
                item.TotalPrice = item.Price * item.Qty;
            }
        }

        protected async Task UpdateQty_Input(int id)
        {
            await Js.InvokeVoidAsync("MakeupdateQtyButtonVisible", id, true);
        }
        private void CartChanged()
        {
            CalculateCartSummaryTotals();
            SetTotalQuantity();
            shoppingCartService.RaiseEventOnShoppingCartCharged(TotalQuantity);
        }
    }
}
