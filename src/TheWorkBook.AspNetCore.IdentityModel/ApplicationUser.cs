using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TheWorkBook.AspNetCore.IdentityModel
{
    public class ApplicationUser : IApplicationUser
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ApplicationUser(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                return GetUserId();
            }
        }

        private int? GetUserId()
        {
            int? userkey = null;
            string userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userId, out int key))
            {
                userkey = key;
            }

            return userkey;
        }
    }
}
