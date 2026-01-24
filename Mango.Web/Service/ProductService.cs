using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service;

public class ProductService(IBaseService _baseService) : IProductService
{
    public async Task<ResponseDto?> CreateProductsAsync(ProductDto productDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = productDto,
            Url = SD.ProductAPIBase + "/api/ProductAPI",
            ContentType = SD.ContentType.MultipartFormData
        });
    }

    public async Task<ResponseDto?> DeleteProductsAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.DELETE,
            Url = SD.ProductAPIBase + "/api/ProductAPI/" + id
        });
    }

    public async Task<ResponseDto?> GetAllProductsAsync()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.ProductAPIBase + "/api/ProductAPI"
        });
    }



    public async Task<ResponseDto?> GetProductByIdAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.ProductAPIBase + "/api/ProductAPI/" + id
        });
    }

    public async Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.PUT,
            Data = productDto,
            Url = SD.ProductAPIBase + "/api/ProductAPI",
            ContentType = SD.ContentType.MultipartFormData
        });
    }
}
