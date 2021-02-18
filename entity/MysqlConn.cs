using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace blog_management
{
    class MysqlConn
    {
        private static string connString = "server=localhost;database=blog;uid=root;pwd=1234567;IgnorePrepare=true;";//数据连接字段
        private static MySqlConnection mySqlConnection = new MySqlConnection(connString);
        private static MySqlConnection mySqlConnectionRead = new MySqlConnection(connString);
        private MysqlConn() {

        }
        public static MySqlConnection getConn() {
            return mySqlConnection;
            
        }
        public static MySqlConnection getConnRead() {
            return mySqlConnectionRead;
        }
        public static void  closeConn() {

            mySqlConnection.Close();

        }
        public static void  closeConnRead() {

            mySqlConnectionRead.Close();

        }

    }
}
