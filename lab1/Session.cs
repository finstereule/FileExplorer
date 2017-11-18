using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class Session
    {
        public int ID;
        public string Username { get; set; }
        public DateTime dateTime { get; set; }
        public string Path { get; set; }

        public Session(int ID, string username, DateTime dateTime, string path ) 
        {
            this.ID = ID;
            this.Username = username;
            this.dateTime = dateTime;
            this.Path = path;
        }

        public Session()
        {

        }
    }
}
