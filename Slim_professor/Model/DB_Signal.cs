using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Slim_professor.Model
{
    class DB_Signal
    {
        DBManager db;

        public enum FIELD{
            std_id, sub_id, status, date, END
        }

        public DB_Signal(DBManager _dbm)
        {
            db = _dbm;
        }
    }
}
