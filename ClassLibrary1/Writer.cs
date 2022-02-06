using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary11
{
    public class Writer
    {
        //СПИСОК ТАБЛИЦ С КОТОРЫМИ РАБОТАЕМ 
        public static readonly string[] tables = new string[]
        {
       // "Users", 
        "Table_3"
        };


        //ЗАШИФРОВАТЬ НЕ ЗАШИФРОВАННЫЕ ТАБЛИЦЫ
        public static void ReCryptoAll()
        {
            using (SqlConnection conn = new SqlConnection("context connection = true"))
            {
                conn.Open();


                foreach (string table in tables)
                {

                    //ВЫБИРАЕМ СТРОКИ БЕЗ ПОМЕТКИ 
                    SqlCommand cmd = new SqlCommand(
                   "SELECT COUNT(*) FROM " + table + " WHERE new = '' ", conn);


                    if (int.Parse(cmd.ExecuteScalar().ToString()) > 0)
                    {

                        //ПОЛУЧАЕМ СПИСОК ПОЛЕЙ ТАБЛИЦЫ 
                        SqlCommand fieldList = new SqlCommand(
                                    "SELECT * FROM syscolumns sc JOIN sysobjects so ON sc.id = so.id WHERE so.Name = '" + table + "'", conn);

                        List<string> fields = new List<string>();
                        List<string> crypto = new List<string>();
                        //ДЕЛАЕМ ЗАПРОС 
                        using (SqlDataReader fieldReader = fieldList.ExecuteReader())
                        {
                            //ПЕРЕБИРАЕМ СТРОКИ 
                            while (fieldReader.Read())
                            {

                                //В ПЕРВОЙ КОЛОНКЕ ИМЯ ПОЛЯ 
                                fields.Add(fieldReader.GetSqlString(0).ToString());
                            }
                        }

                        //ВЫБИРАЕМ СТРОКИ , КОТОРЫЕ НЕ ЗАШИФРОВАНЫ 
                        SqlCommand data = new SqlCommand("SELECT * FROM " + table + " WHERE new = '' ", conn);


                        using (SqlDataReader fieldReader = data.ExecuteReader())
                        {
                        
                            //ПЕРЕБИРАЕМ НЕ ЗАШИФРОВАННЫЕ СТРОКИ 
                            while (fieldReader.Read())
                            {
                                string id = "-1";
                                //ГЕНЕРАЦИЯ КЛЮЧА
                                byte[] key = Keys.Generate();
                                List<string> dataList = new List<string>();

                                //ПЕРЕБИРАЕМ ПОЛЯ 
                                for (int i = 0; i < fieldReader.FieldCount; i++)
                                {
                                    //ВЫПИСЫВАЕМ ИМЯ ПОЛЯ 
                                    string fieldName = fields[i];
                                    //ЕСЛИ ID 
                                    if (fieldName == "id")
                                    {
                                        //ТО ЗАПОМИНАЕМ ЕГО И ИДЕМ ДАЛЬШЕ 
                                        id = fieldReader.GetValue(i).ToString();
                                        continue;
                                    }


                                    //СТАВИМ ПОМЕТКУ, ЧТО СТРОКА ЗАШИФРОВАНА 
                                    if (fieldName == "new")
                                    {
                                        dataList.Add(String.Format("{0}='{1}'",
                                                fieldName,
                                              "Y"
                                                ));
                                        continue;
                                    }

                                    //ВКЛЮЧАЕМ ШИФРОВАНИЕ 
                                    Crypto engine = new Crypto();

                                    //СОБИРАЕМ В МАССИВ ДЛЯ UPDATE
                                    dataList.Add(String.Format("{0}='{1}'",
                                        fieldName,

                                       //ШИФРОВАНИЕ 
                                      engine.encrypt(fieldReader.GetValue(i).ToString(), key)
                                     ));

                                }

                                Keys.Set(table, id, key);
                                //ФОРМИРУЕМ UPDATE ПОД КАЖДУЮ СТРОКУ В МАССИВ 
                                crypto.Add(String.Format("UPDATE {0} SET {1} WHERE id = '{2}'", table, String.Join(",", dataList.ToArray()), id));

                            }
                        }


                        //ВЫПОЛНЯЕМ КАЖДЫЙ ЗАПРОС 
                        foreach (string sql in crypto)
                        {
                            SqlCommand update = new SqlCommand(sql, conn);
                            update.ExecuteNonQuery();
                        }




                    }
                }



            }
        }

    }
}
