using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary11
{
    public class Reader
    {


        //БАЗОВЫЙ МЕТОД
        public static IEnumerable GetTable(string table)
        {
            ArrayList resultCollection = new ArrayList();


            //ПОДКЛЮЧАЕМСЯ К ТЕКУЩЕЙ БД
            using (SqlConnection conn
                = new SqlConnection("context connection=true"))
            {
                conn.Open();
                //ВЫБИРАЕМ ВСЕ ПОЛЯ 
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM " + table + ";", conn);

                //ВЫПОЛНЯЕМ ЗАПРОС 
                using (SqlDataReader dataReader = cmd.ExecuteReader())
                {

                    //ПЕРЕБОР СТРОК
                    while (dataReader.Read())
                    {
                        byte[] key = Keys.Get(table, dataReader.GetInt32(dataReader.FieldCount - 1).ToString());
                        List<SqlString> fieldsList = new List<SqlString>(10);
                        
                        
                        
                        //ПЕРЕБИРАЕМ ПОЛЯ 
                        for (int i = 0; i < dataReader.FieldCount - 1; i++)
                        {
                            //СОЗДАЕМ ОБЪЕКТ КРИПТО
                            Crypto engine = new Crypto();

                            //СОБИРАЕМ РАСШИФРОВАННЫЕ ДАННЫЕ 
                            fieldsList.Add(

                               engine.decrypt(
                                dataReader.GetString(i), key
                                )
                           );
                        }

                        //ДОБИВАЕМ ДО 10 
                        for (int i = dataReader.FieldCount; i < 10; i++)
                        {
                            fieldsList.Add("");
                        }

                        ///СОБИРАЕМ В СТРОКУ 
                        resultCollection.Add(new Row(fieldsList));

                    }
                }

            }
            
            //ВОЗВРАЩАЕМ КОЛЕКЦИЮ СТРОК 
            return resultCollection;


        }

    }
}
