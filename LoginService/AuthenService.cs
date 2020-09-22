using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class AuthenService : IAuthenService
    {
        string jwtSecret;
        int jwtLifeSpan;

        public AuthenData GetAuthenData(string id)
        {
            throw new NotImplementedException();
        }

        public string HashPassword(string pwd)
        {
            throw new NotImplementedException();
        }

        public bool IsPwdValid(string pwd, string h_pwd)
        {
            throw new NotImplementedException();
        }
    }
}
