using Microsoft.AspNetCore.Http;
using System;

namespace PWRekruter.Services
{
    public enum UserType
    {
        Kandydat,
        Rekruter
    }

    public interface ILoginService
    {
        int GetUserId();
        UserType GetUserType();

        void Login(int userId, UserType type);
        void Logout();
    }

    public class LoginService : ILoginService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void Login(int userId, UserType type)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append("UserId", userId.ToString());
            _httpContextAccessor.HttpContext.Response.Cookies.Append("UserType", type.ToString());
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("UserType");
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("UserId");
        }

        public int GetUserId()
        {
            string userIdCookieValue = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
            return Convert.ToInt32(userIdCookieValue);
        }

        public UserType GetUserType()
        {
            string userTypeCookieValue = _httpContextAccessor.HttpContext.Request.Cookies["UserType"];
            return (UserType)Enum.Parse(typeof(UserType), userTypeCookieValue);
        }
    }
}
