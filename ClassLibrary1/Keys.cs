using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary11
{
    class Keys
    {

        public static byte[] Get(string table, string id)
        {

            string result ="";
            using (WebClient client = new WebClient())
            {
                result = client.DownloadString( String.Format("https://tasty-catalog.ru/keys/get/?table={0}&id={1}", table,id));
            }
            return Convert.FromBase64String(result);
        }



        public static void Set(string table, string id, byte[] key)
        {
            using(WebClient client = new WebClient())
            {
                string result = client.DownloadString(String.Format("https://tasty-catalog.ru/keys/set/?table={0}&id={1}&key={2}", table, id, Convert.ToBase64String(key) ));
            }
        }



        //генерация случайного ключа
        public static byte[] Generate()
        {
            Random rnd = new Random();
            List<byte> result = new List<byte>();
            for(int i = 0; i< 32; i++)
            {
                result.Add(
                   (byte) rnd.Next(0, 255)
                    );
            }
            return result.ToArray();
        }
    }
}
