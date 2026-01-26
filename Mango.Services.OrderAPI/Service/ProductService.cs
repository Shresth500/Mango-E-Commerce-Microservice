using Mango.Services.OrderAPI.Models.Dto;
using Mango.Services.OrderAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Service.ShoppingCartAPI.Service;

public class ProductService(IHttpClientFactory httpClientFactory) : IProductService
{
    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        var client = httpClientFactory.CreateClient("Product");
        var response = await client.GetAsync($"/api/ProductAPI");
        var apiContent = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        if(resp!.IsSuccess)
        {
            return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result)!)!;
        }
        return new List<ProductDto>();
    }
}
