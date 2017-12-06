using System;
using System.Text;
using System.IO;

namespace lab1
{
    public partial class Logger
    {
        public static void Log(string message, string mParam)
        {

            using (var context = new FileManagerEntities())
            {
                var dbLogg = new DbLoggs();

                try
                {
                    dbLogg.MDate = DateTime.Now.ToString("HH:mm:ss.ms");
                    dbLogg.Massage = message;
                    dbLogg.Param = mParam; //MSG - regular message; ERR - error; PATH - selected path
                    if(StaticResources.CurrUserId != 0)
                    dbLogg.UserId = StaticResources.CurrUserId; //if 0 then 'anonymous'
                    context.DbLoggs.AddObject(dbLogg);
                    context.SaveChanges();
                }
                catch
                {
                }
            }
        }
    }
}
