using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class AuthenData
    {
        public string Token { get; set; }
        public long TokenExpirationTime { get; set; }
        public string Id { get; set; }
    }
}
