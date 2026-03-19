using HelloRazorPage.Models;

namespace HelloRazorPage.Services
{
    public class UserService
    {
        private List<User> _users;
        private int _lastId = 0;

        public UserService()
        {
            _users = new();
            Startup();
        }

        public void GetDataFromSource()
        {
            _users.Add(new User() { Id = GetCurrentNextId(), Login = "log1", Password = "1111" });
            _users.Add(new User() { Id = GetCurrentNextId(), Login = "log2", Password = "2222" });
            _users.Add(new User() { Id = GetCurrentNextId(), Login = "log3", Password = "3333" });
        }

        public void Startup()
        {
            GetDataFromSource();
        }

        private int GetCurrentNextId() => _lastId++;

        public List<User> GetAll() => _users;

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public User Add(User user)
        {
            user.Id = GetCurrentNextId();
            _users.Add(user);

            return user;
        }

        public bool Update(User user)
        {
            var found_user = GetById(user.Id);

            if (found_user == null)
                return false;

            found_user.Login = user.Login;
            found_user.Password = user.Password;

            return true;
        }

        public bool DeleteById(int id)
        {
            var found_user = GetById(id);

            if (found_user == null)
                return false;

            _users.Remove(found_user);
            return true;
        }
    }
}