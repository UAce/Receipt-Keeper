using System.Security.Claims;
using Domain.Models;

namespace Application.Services
{
    public class HttpContextService(IHttpContextAccessor httpContextAccessor) : IHttpContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public CurrentUser CurrentUser
        {
            get
            {
                if (
                    _httpContextAccessor.HttpContext?.Items.TryGetValue(
                        "CurrentUser",
                        out var userObj
                    ) != null
                    && userObj is CurrentUser user
                )
                {
                    return user;
                }

                throw new Exception("Current User not found in HttpContext");
            }
        }

        public string IdentityId
        {
            get
            {
                var identityId = _httpContextAccessor
                    .HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)
                    ?.Value;

                if (identityId != null && identityId is not null)
                {
                    return identityId;
                }

                throw new Exception("IdentityId not found in HttpContext");
            }
        }
    }
}
