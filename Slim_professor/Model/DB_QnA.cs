using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Slim_professor.Model
{
    class DB_QnA
    {
        DBManager db;

        public enum FIELD{
            id, sub_id, std_id, std_name, title, content, END
        }

        public DB_QnA(DBManager _dbm)
        {
            db = _dbm;
        }
    }
}
