using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace Slim_professor.Model
{
    class DB_User
    {
        DBManager db;

        public enum FIELD{
            user_id, user_name, group, sub_ids, pw, auth, END
        }

        public DB_User(DBManager _dbm)
        {
            db = _dbm;
        }

        public object[] SelectStudent(String auth)
        {
            string sql = "SELECT * FROM user WHERE auth=@arg1";
            List<object> args = new List<object>();
            args.Add(auth);

            List<object[]> result = SearchDatas(sql, args);
            if (result.Count == 0)
                return null;
            else
                return result[0];
        }

        public object[] SelectUser(string id)
        {
            string sql = "SELECT * FROM user WHERE user_id=@arg1";  
            List<object> args = new List<object>();
            args.Add(id);

            List<object[]> result = SearchDatas(sql, args);
            if (result.Count == 0)
                return null;
            else
                return result[0];
        }

        public object[] SelectUser(string id, string pw)
        {
            string sql = "SELECT * FROM user WHERE user_id=@arg1 AND pw=password(@arg2)";   // DB에 password로 암호화 돼서 저장됐기 때문에 복호화로 password함수 써야한다.
            List<object> args = new List<object>();
            args.Add(id);
            args.Add(pw);

            List<object[]> result = SearchDatas(sql, args);
            if (result.Count == 0)
                return null;
            else 
                return result[0];
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
                    for (int i = 0; i < args.Count;i++ )
                        cmd.Parameters.AddWithValue("@arg"+(i+1), args[i]);
                    
                    cmd.ExecuteNonQuery();
                    reader = cmd.ExecuteReader();

                    cmd.Connection = db.Connection;
                }

                while (reader.Read())
                {
                    object[] items = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
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

        internal object SelectUser(object user_id)
        {
            throw new NotImplementedException();
        }
    }
}
