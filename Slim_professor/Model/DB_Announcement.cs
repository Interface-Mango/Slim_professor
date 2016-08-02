using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Slim_professor.Model
{
    class DB_Announcement
    {
        DBManager db;

        public enum FIELD{
            id, title, content, sub_id, sub_name, date, END
        }

        public DB_Announcement(DBManager _dbm)
        {
            db = _dbm;
        }
    }
}
