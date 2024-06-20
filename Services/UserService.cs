namespace CRUDApi.Services
{
    public class UserService
    {

        private readonly Dictionary<string, string> _users; // Simulated user storage

        public UserService()
        {
            _users = new Dictionary<string, string>(); // Initialize user dictionary (replace with database logic)
        }

        public bool RegisterUser(string username, string password)
        {
            if (_users.ContainsKey(username))
            {
                return false; // User already exists
            }

            _users[username] = password; // Add new user
            return true;
        }
    }
}
