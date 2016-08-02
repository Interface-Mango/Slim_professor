﻿using System;
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
            sub_id, sub_name, lectureler_id, time, location, END
        }

        public DB_Subject(DBManager _dbm)
        {
            db = _dbm;
        }

        public object[] SelectSubjectListForStudent(int sub_id)
        {
            string sql = "SELECT * FROM subject WHERE sub_id=@arg1";   
            List<object> args = new List<object>();
            args.Add(sub_id);

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
    }
}