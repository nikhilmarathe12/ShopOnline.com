using Microsoft.AspNetCore.Components;
using ShopOnlineModels.Dto;
using ShopOnlineWeb.Services.Contracts;

namespace ShopOnlineWeb.Shared
{
    public class ProductCategoryNavMenuBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        public IEnumerable<ProductCategoryDto> productCategoryDtos { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                productCategoryDtos = await ProductService.GetProductCategories();
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }

    }
}
