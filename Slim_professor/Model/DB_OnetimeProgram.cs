using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Slim_professor.Model
{
    class DB_OnetimeProgram
    {
        DBManager db;

        public enum FIELD
        {
            id, process_name, sub_id, sub_name, check_field, END
        }

        public DB_OnetimeProgram(DBManager _dbm)
        {
            db = _dbm;
        }
        public List<object[]> SelectOneTimeList(string process_name)
        {
            string sql = "SELECT * FROM onetime_program WHERE process_name=@arg1";
            List<object> args = new List<object>();
            args.Add(process_name);

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
                    for (int i = 0; i<args.Count; i++)
                        cmd.Parameters.AddWithValue("@arg" + (i + 1), args[i]);

                    cmd.ExecuteNonQuery();
                    reader = cmd.ExecuteReader();

                    cmd.Connection = db.Connection;
                }

                while (reader.Read())
                {
                    object[] items = new object[reader.FieldCount]; // columns 개수
                    for (int i = 0; i<reader.FieldCount; i++)
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


        public bool InsertOneTime(string process_name, int sub_id, string sub_name, int check)
        {
            string sql = "INSERT INTO onetime_program(process_name, sub_id, sub_name, check_field) VALUES(@arg1, @arg2, @arg3, @arg4)";
            List<object> args = new List<object>();
            args.Add(process_name);
            args.Add(sub_id);
            args.Add(sub_name);
            args.Add(check);


            try
            {
                db.Connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, db.Connection))
                {
                    for (int i = 1; i <= 4; i++)
                    {
                        cmd.Parameters.AddWithValue("@arg" + i, args.ElementAt(i - 1));
                    }
                    cmd.ExecuteNonQuery();
                }
                db.Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  // For Debugging
                return false;    // 삽입 오류시 false 반환
            }

            return true;

        }

        public bool DeleteOneTime(int sub_id)
        {
            string sql = "DELETE FROM onetime_program where sub_id=@arg1";
            
            try
            {
                db.Connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, db.Connection))
                {
                    cmd.Parameters.AddWithValue("@arg1", sub_id);
                    cmd.ExecuteNonQuery();
                }
                db.Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  // For Debugging
                return false;    // 제거 오류시 false 반환
            }

            return true;
        }

        //aaa - UpdateOneTime. UpdateOneTime1 둘다 sub_id를 받아서 수정하게끔!!!
        public bool UpdateOneTime(string process_name, int check_field, int sub_id)
        {
            string sql="";
            if(check_field == 0)
                sql = "UPDATE onetime_program SET check_field=@arg1 WHERE process_name!=@arg2 AND sub_id=@arg3";
            else
                sql = "UPDATE onetime_program SET check_field=@arg1 WHERE process_name=@arg2 AND sub_id=@arg3";
            
            try
            {
                db.Connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, db.Connection))
                {
                    cmd.Parameters.AddWithValue("@arg1", check_field);
                    cmd.Parameters.AddWithValue("@arg2", process_name);
                    cmd.Parameters.AddWithValue("@arg3", sub_id);

                    cmd.ExecuteNonQuery();
                }
                db.Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  // For Debugging
                return false;    // 제거 오류시 false 반환
            }

            return true;
        }

    }

    

}
