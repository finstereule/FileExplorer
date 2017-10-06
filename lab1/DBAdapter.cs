using System.Collections.Generic;

namespace lab1
{
    public static class DBAdapter
    {
        public static List<User> Users { get; set; }

        static DBAdapter()
        {
            Users = new List<User>
            {
                new User("1", "1")
            };
        }
    }
}
