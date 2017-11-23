using System.IO;
using System.Xml.Serialization;

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
        /*
        public void Save(string FileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(User));
                XML.Serialize(stream, this);
            }
        }

    */
        public User()
        {

        }
    }
}
