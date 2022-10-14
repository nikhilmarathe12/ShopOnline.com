﻿using Microsoft.AspNetCore.Components;
using ShopOnlineModels.Dto;
using ShopOnlineWeb.Services.Contracts;

namespace ShopOnlineWeb.Pages
{
    public class ProductByCategoryBase : ComponentBase
    {
        [Parameter]
        public int CategoryId { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

        public string CategoryName { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                //Products = await ProductService.GetItemsByCategory(CategoryId);
                Products = await GetProductCollectionByCategoryId(CategoryId);
                if (Products != null && Products.Count() > 0)
                {
                    var productDto = Products.FirstOrDefault(p => p.CategoryId == CategoryId);
                    if(productDto != null)
                    {
                        CategoryName = productDto.CategoryName;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async Task<IEnumerable<ProductDto>> GetProductCollectionByCategoryId(int categoryId)
        {
            var productCollection = await ManageProductsLocalStorageService.GetCollection();

            if(productCollection != null)
            {
                return productCollection.Where(p => p.CategoryId == categoryId);
            }
            else
            {
                return await ProductService.GetItemsByCategory(categoryId);
            }
        }

    }
}
