using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slim_professor.Model
{
    class DB_AllProgram
    {
        DBManager db;

        public enum FIELD
        {
            id, process_name, title_name, sub_id, lecturer_id, lecture_time, red_green, is_all_red, END
        }

        public DB_AllProgram(DBManager _dbm)
        {
            db = _dbm;
        }
    }
}
