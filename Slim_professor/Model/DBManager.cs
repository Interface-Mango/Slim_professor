using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace Slim_professor.Model
{
    class DBManager
    {
        private MySqlConnection _Connection;
        public MySqlConnection Connection
        {
            get { return _Connection; }
        }

        private bool _IsDBOpen;
        public bool IsDBOpen
        {
            get { return _IsDBOpen; }
        }


        public DBManager()
        {
            string connectionString = "Server=14.63.196.146; Port=3306; Database=slim_schema; Uid=guest; pwd=m@ngo518";
            _IsDBOpen = false;

            try
            {
                _Connection = new MySqlConnection(connectionString);
                _IsDBOpen = true;
            }
            catch (Exception ex)
            {
                _IsDBOpen = false;
                Console.WriteLine(ex.Message);
            }

        }

    }
}
