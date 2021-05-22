using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessApplication.TourManagement.Api.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }

        public UserInfoService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            var currentContext = this.httpContextAccessor.HttpContext;
            if(currentContext == null || !currentContext.User.Identity.IsAuthenticated)
            {
                UserId = "N/A";
                FirstName = "N/A";
                LastName = "N/A";
                Role = "Administrator";
                return;
            }

            UserId = (currentContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value) ?? "N/A";
            FirstName = (currentContext.User.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value) ?? "N/A";
            LastName = (currentContext.User.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value) ?? "N/A";
            Role = (currentContext.User.Claims.FirstOrDefault(c => c.Type == "role")?.Value)?? "Administrator";
        }


    }
}
