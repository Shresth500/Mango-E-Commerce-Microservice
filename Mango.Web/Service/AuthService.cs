using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service;

public class AuthService(IBaseService _baseService) :IAuthService
{
    public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = SD.ApiType.POST,
            Data = registrationRequestDto,
            Url = SD.AuthAPIBase + "/api/AuthAPI/AssignRole"
        }, withBearer : false);
    }

    public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiType.POST,
            Data = loginRequestDto,
            Url = SD.AuthAPIBase + "/api/AuthAPI/login"
        },withBearer:false);
    }

    public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
    {
        Console.WriteLine(SD.AuthAPIBase);
        return await _baseService.SendAsync(new RequestDto
        {
            ApiType = SD.ApiType.POST,
            Data = registrationRequestDto,
            Url = SD.AuthAPIBase + "/api/AuthAPI/register"
        }, withBearer: false);
    }
}
