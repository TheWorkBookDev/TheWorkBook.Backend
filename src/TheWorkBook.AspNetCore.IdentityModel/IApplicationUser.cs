using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorkBook.AspNetCore.IdentityModel
{
    public interface IApplicationUser
    {
        int? UserId { get; }
    }
}
