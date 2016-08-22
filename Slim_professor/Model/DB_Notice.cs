using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Slim_professor.Model
{
    class DB_Notice
    {
        DBManager db;

        public enum FIELD
        {
            id, title, content, sub_id, date, END
        }

        public DB_Notice(DBManager _dbm)
        {
            db = _dbm;
        }

        public List<object[]> SelectNoticeList(int sub_id)
        {
            string sql = "SELECT * FROM announcement WHERE sub_id=@arg1 ORDER BY id DESC";
            List<object> args = new List<object>();
            args.Add(sub_id);

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
                    for (int i = 0; i < args.Count; i++)
                        cmd.Parameters.AddWithValue("@arg" + (i + 1), args[i]);

                    cmd.ExecuteNonQuery();
                    reader = cmd.ExecuteReader();

                    cmd.Connection = db.Connection;
                }

                while (reader.Read())
                {
                    object[] items = new object[reader.FieldCount]; // columns 개수
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
        
        public bool InsertNotice(string title, string content, int sub_id)
        {
            string sql = "INSERT INTO announcement(title, content, sub_id, date) VALUES(@arg1, @arg2, @arg3, NOW())";
            List<object> args = new List<object>();
            args.Add(title);
            args.Add(content);
            args.Add(sub_id);

            try
            {
                db.Connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, db.Connection))
                {
                    for (int i = 1; i <= 3;i++ )
                    {
                        cmd.Parameters.AddWithValue("@arg" + i, args.ElementAt(i-1));
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

        public bool UpdateNotice(int id, string title, string content)
        {
            string sql = "UPDATE announcement SET title=@arg1, content=@arg2 WHERE id=@arg3";
            List<object> args = new List<object>();
            args.Add(title);
            args.Add(content);
            args.Add(id);

            try
            {
                db.Connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, db.Connection))
                {
                    cmd.Parameters.AddWithValue("@arg1", title);
                    cmd.Parameters.AddWithValue("@arg2", content);
                    cmd.Parameters.AddWithValue("@arg3", id);
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

        public bool DeleteNotice(int id)
        {
            string sql = "DELETE FROM announcement WHERE id=@arg";

            try
            {
                db.Connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, db.Connection))
                {
                        cmd.Parameters.AddWithValue("@arg", id);
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
