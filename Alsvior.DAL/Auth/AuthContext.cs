using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alsvior.DAL.Auth
{
    public class AuthContext: IdentityDbContext<IdentityUser>
    {
        public AuthContext(): base("Auth")
        {

        }
    }
}
