using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Slim_professor.Model
{
    class DB_AllProgram
    {
        DBManager db;

        public enum FIELD
        {
            id, process_name, title_name, sub_id, lecturer_id, lecture_time, red_green, is_all_red, END
        }

        public DB_AllProgram(DBManager _dbm)
        {
            db = _dbm;
        }
 
         public List<object[]> SelectAllProgramList(int sub_id, int red_green)
         {
             string sql = "SELECT * FROM all_program WHERE sub_id=@arg1 AND red_green=@arg2 ";
             List<object> args = new List<object>();
             args.Add(sub_id);
             args.Add(red_green);
 
 
             List<object[]> result = SearchDatas(sql, args);
             if (result.Count == 0)
                 return null;
             else
                 return result;
         }
 
         public List<object[]> SearchDatas(string sql, List<object> args)
         {
             List<object[]> recordList = new List<object[]>();
 
             try
             {
                 db.Connection.Open();
                 MySqlDataReader reader;
                 using (MySqlCommand cmd = new MySqlCommand(sql, db.Connection))
                 {
                     for (int i = 0; i<args.Count; i++  )
                         cmd.Parameters.AddWithValue("@arg"  + (i  + 1), args[i]);
 
                     cmd.ExecuteNonQuery();
                     reader = cmd.ExecuteReader();
 
                     cmd.Connection = db.Connection;
                 }
 
                 while (reader.Read())
                 {
                     object[] items = new object[reader.FieldCount]; // columns 개수
                     for (int i = 0; i<reader.FieldCount; i++  )
                     {
                         items[i] = reader[i];
                     }
                     recordList.Add(items);
                 }
 
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
             }
             finally
             {
                 db.Connection.Close();
             }
 
             return recordList;
         }
     }
 }

    }
}
