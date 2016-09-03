using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Slim_professor.Model
{
    class DB_Subject
    {
        DBManager db;

        public enum FIELD{
            sub_id, sub_name, lectureler_id, time, location, ipaddr, is_processing, END
        }

        public DB_Subject(DBManager _dbm)
        {
            db = _dbm;
        }

        public List<object[]> SelectSubjectList(int lec_id)
        {
            string sql = "SELECT * FROM subject WHERE lectureler_id=@arg1";   
            List<object> args = new List<object>();
            args.Add(lec_id);

            List<object[]> result = SearchDatas(sql, args);
            if (result.Count == 0)
                return null;
            else 
                return result;
        }

        private List<object[]> SearchDatas(string sql, List<object> args)
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

        // 채팅방 열었을 때 IP와 Port 등록하는 함수
        public bool UpdateIpaddr(int sub_id, string ipaddr, int port)
        {
            string sql = "UPDATE subject SET ipaddr=@arg1, port=@arg2 WHERE sub_id=@arg3";

            try
            {
                db.Connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, db.Connection))
                {
                    cmd.Parameters.AddWithValue("@arg1", ipaddr);
                    cmd.Parameters.AddWithValue("@arg2", port);
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

        // 강사가 과목 입장시 is_processing (수업 진행 관련 플래그 변경)
        public bool UpdateIsProcessing(int sub_id, int isProcessing)
        {
            string sql = "UPDATE subject SET is_processing=@arg1 WHERE sub_id=@arg2";

            try
            {
                db.Connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, db.Connection))
                {
                    cmd.Parameters.AddWithValue("@arg1", isProcessing);
                    cmd.Parameters.AddWithValue("@arg2", sub_id);
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
