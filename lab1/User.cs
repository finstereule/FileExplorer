namespace lab1
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User(string username, string password)
        {
            this.Password = password;
            this.Username = username;
        }

        public User()
        {

        }
    }
}
