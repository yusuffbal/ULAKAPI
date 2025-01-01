using Entities.Dtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        string Register(UserRegisterDto user, string password);
        string Login(string email, string password);
        string GenerateJwtToken(User user);

    }
}
