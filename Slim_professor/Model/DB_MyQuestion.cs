using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Slim_professor.Model
{
    class DB_MyQuestion
    {
        DBManager db;

        public enum FIELD{
            id, std_id, sub_id, content, END
        }

        public DB_MyQuestion(DBManager _dbm)
        {
            db = _dbm;
        }
    }
}
