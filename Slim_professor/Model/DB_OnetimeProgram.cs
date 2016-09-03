using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slim_professor.Model
{
    class DB_OnetimeProgram
    {
        DBManager db;

        public enum FIELD
        {
            id, process_name, title_name, location, sub_id, sub_name, END
        }

        public DB_OnetimeProgram(DBManager _dbm)
        {
            db = _dbm;
        }
    }
}
