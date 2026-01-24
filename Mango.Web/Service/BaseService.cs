using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service;

public class BaseService(IHttpClientFactory httpClientFactory,ITokenProvider _tokenProvider) : IBaseService
{
    public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
    {
        try
        {
            HttpClient httpClient = httpClientFactory.CreateClient();
            HttpRequestMessage httpRequestMessage = new();

            httpRequestMessage.RequestUri = new Uri(requestDto.Url);

            if (withBearer)
            {
                var token = _tokenProvider.GetToken();
                httpRequestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            if (requestDto.ContentType == ContentType.MultipartFormData)
            {
                httpRequestMessage.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("*/*"));

                var content = new MultipartFormDataContent();

                foreach (var prop in requestDto.Data.GetType().GetProperties())
                {
                    var value = prop.GetValue(requestDto.Data);

                    if (value is IFormFile file)
                    {
                        content.Add(
                            new StreamContent(file.OpenReadStream()),
                            prop.Name,
                            file.FileName
                        );
                    }
                    else
                    {
                        content.Add(
                            new StringContent(value?.ToString() ?? ""),
                            prop.Name
                        );
                    }
                }

                httpRequestMessage.Content = content;
            }
            else
            {
                httpRequestMessage.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                if (requestDto.Data != null)
                {
                    httpRequestMessage.Content = new StringContent(
                        JsonConvert.SerializeObject(requestDto.Data),
                        Encoding.UTF8,
                        "application/json"
                    );
                }
            }

            httpRequestMessage.Method = requestDto.ApiType switch
            {
                ApiType.POST => HttpMethod.Post,
                ApiType.PUT => HttpMethod.Put,
                ApiType.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get
            };

            var apiResponse = await httpClient.SendAsync(httpRequestMessage);

            if (!apiResponse.IsSuccessStatusCode)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = apiResponse.StatusCode.ToString()
                };
            }

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseDto>(apiContent);
        }
        catch (Exception ex)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = ex.Message
            };
        }
    }

}
