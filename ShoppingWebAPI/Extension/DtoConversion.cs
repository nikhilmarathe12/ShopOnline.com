using ShopOnlineModels.Dto;
using ShoppingWebAPI.Data;
using ShoppingWebAPI.Entities;

namespace ShoppingWebAPI.Extension
{
    public static class DtoConversion
    {
        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return (from productCategory in productCategories
                    select new ProductCategoryDto
                    {
                        Id = productCategory.Id,
                        Name = productCategory.Name,
                        IconCSS = productCategory.IconCSS,
                    }).ToList();
        }

        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            return (from product in products
                    select new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ImageUrl = product.ImageURL,
                        Price = product.Price,
                        Qty = product.Qty,
                        CategoryId = product.ProductCategory.Id,
                        CategoryName = product.ProductCategory.Name
                    }).ToList();
        }

        public static ProductDto ConvertToDto(this Product products)
        {
            return new ProductDto
            {
                Id = products.Id,
                Name = products.Name,
                Price = products.Price,
                Description = products.Description,
                ImageUrl = products.ImageURL,
                Qty = products.Qty,
                CategoryId = products.ProductCategory.Id,
                CategoryName = products.ProductCategory.Name
            };
        }

        public static IEnumerable<CartItemDto> ConvetToDto(this IEnumerable<CartItem> cartItems,
                                                           IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.ProductId equals product.Id
                    select new CartItemDto
                    {
                        Id = cartItem.Id,
                        ProductId = cartItem.ProductId,
                        ProductName = product.Name,
                        ProductDescription = product.Description,
                        ProductImageUrl = product.ImageURL,
                        CartId = cartItem.CartId,
                        Price = product.Price,
                        Qty = cartItem.Qty,
                        TotalPrice = product.Price * cartItem.Qty
                    });
        }

        public static CartItemDto ConvertToDto(this CartItem cartItem, Product product)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageUrl = product.ImageURL,
                CartId = cartItem.CartId,
                Price = product.Price,
                Qty = cartItem.Qty,
                TotalPrice = product.Price * cartItem.Qty
            };
        }

    }
}
