using Microsoft.AspNetCore.Components;
using ShopOnlineModels.Dto;

namespace ShopOnlineWeb.Pages
{
    public class DisplayProductBase : ComponentBase
    {
        [Parameter]
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
