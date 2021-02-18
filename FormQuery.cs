using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace blog_management
{
    public partial class FormQuery : Form
    {
        public FormQuery()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection mySqlConnectionRead = MysqlConn.getConnRead();
            MySqlCommand mySqlCommand;
            String sql;
            try
            {
                if (textBox1.Text != "")
                {
                    Convert.ToInt32(textBox1.Text);
                    sql = "select * from t_type where id="+textBox1.Text;
                }
                else {
                    sql = "select * from t_type";
                }
                mySqlCommand = new MySqlCommand(sql, mySqlConnectionRead);
                mySqlConnectionRead.Open();

                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        textBox2.Text += mySqlDataReader.GetString(i);
                        textBox2.Text += "  ";
                    }
                    textBox2.Text += "\r\n";
                }

            }
            catch(Exception ex) {

                textBox2.Text += "The data does not exists:"+ex.Message;
                textBox2.Text += "\r\n";

            }
            finally
            {
                MysqlConn.closeConnRead();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            MySqlConnection mySqlConnectionRead = MysqlConn.getConnRead();
            MySqlCommand mySqlCommand;
            String sql;
            try
            {
                if (textBox3.Text != "")
                {
                    Convert.ToInt32(textBox3.Text);
                    sql = "select title,create_time,t.name from t_blog b,t_type t where b.type_id = t.id and b.id=" + textBox3.Text;
                }
                else
                {
                    sql = "select title,create_time,t.name from t_blog b,t_type t where b.type_id = t.id;";
                }
                mySqlCommand = new MySqlCommand(sql, mySqlConnectionRead);
                mySqlConnectionRead.Open();

                MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

                while (mySqlDataReader.Read())
                {
                    for (int i = 0; i <= 2; i++)
                    {
                        textBox2.Text += mySqlDataReader.GetString(i);
                        textBox2.Text += "  ";
                    }
                    textBox2.Text += "\r\n";
                }

            }
            catch (Exception ex)
            {

                textBox2.Text += "The data does not exists:" + ex.Message;
                textBox2.Text += "\r\n";

            }
            finally
            {
                MysqlConn.closeConnRead();
            }
        }
    }
}
