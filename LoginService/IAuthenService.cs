using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public interface IAuthenService
    {
        public AuthenData GetAuthenData(string id);
        public string HashPassword(string pwd);
        public bool IsPwdValid(string pwd, string h_pwd);
    }
}
