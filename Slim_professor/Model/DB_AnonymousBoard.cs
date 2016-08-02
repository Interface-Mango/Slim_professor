using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Slim_professor.Model
{
    class DB_AnonymousBoard
    {
        DBManager db;

        public enum FIELD{
            id, sub_id, std_id, content, END
        }

        public DB_AnonymousBoard(DBManager _dbm)
        {
            db = _dbm;
        }
    }
}
