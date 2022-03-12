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

            Claim claim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserKey");

            if (claim != null)
            {
                if (int.TryParse(claim.Value, out int key))
                    userkey = key;
            }

            return userkey;
        }
    }
}
