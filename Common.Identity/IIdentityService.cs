using Common.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Identity
{
    public interface IIdentityService
    {
        Task<string> GenerateJwtToken(LoginModel login);
    }
}
