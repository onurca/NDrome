using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ndrome.Common;
using Ndrome.Model.Authentication;
using Ndrome.Service.Interfaces;

namespace Ndrome.Service.Services
{
    public class UserService : Service, IUserService
    {
        readonly IRepository<User> _userRepository;

        public UserService()
        {
            _userRepository = UnitOfWork.GetRepository<User>();
        }

        public User Authenticate(string username, string password)
        {
            var user = _userRepository.Get(x => x.Username == username && x.Password == password);

            if (user == null)
                return null;

            user.Token = new TokenService().GenerateToken(username);

            user.Password = null;

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll().AsEnumerable().Select(x => {
                x.Password = null;
                return x;
            });
        }
    }
}
