namespace lab1
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User(string username, string password) //ім'я та пароль користувача
        {
            this.Password = password;
            this.Username = username;
        }

        public User()
        {

        }
    }
}
