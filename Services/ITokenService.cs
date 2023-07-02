using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicAPIwithJWT.Entities;

namespace BasicAPIwithJWT.Services;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
