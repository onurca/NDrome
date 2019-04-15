using System.Collections.Generic;
using Ndrome.Model.Authentication;

namespace Ndrome.Service.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }
}
