using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace blog_management
{
    public partial class FormMain : Form
    {
        private string rootRoad = @"D:\Microsoft VScode\markdown";
        internal static List<Type> dictionaryList = new List<Type>();
        internal static List<Blog> fileNameList = new List<Blog>();
        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //查询窗口
            FormQuery form = new FormQuery();
            /*form.MdiParent = this;*/
            form.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //先获取根目录下面的所有文件夹名称
            string[] dirList = Directory.GetDirectories(rootRoad);
            //获取每个文件夹名称
            //因为有.git和.vscode文件夹所以从i=2开始
            for (int i = 2; i < dirList.Length; i++)
            {
                //可实例化的文件夹流操作,减少I/O
                DirectoryInfo info = new DirectoryInfo(dirList[i]);
                //获取文件夹名称
                string fileName = info.Name;
                TreeNode rootNode = new TreeNode(fileName);
                //添加根节点
                treeView1.Nodes.Add(rootNode);

                //获取子节点
                SetNode(info.FullName, rootNode);

                //添加目录
                dictionaryList.Add(new Type(fileName));
            }
            ImageList image = new ImageList();
            image.Images.Add(Image.FromFile("folder.jpg"));
            treeView1.ImageList = image;

            treeView1.SelectedImageIndex = 1;
        }

        void SetNode(string fullRoad, TreeNode nodes)
        {
            //先获取根目录下面的所有文件夹名称
            string[] dirList = Directory.GetDirectories(fullRoad);
            for (int i = 0; i < dirList.Length; i++)
            {
                DirectoryInfo info = new DirectoryInfo(dirList[i]);
                string fileName = info.Name;
                TreeNode rootNode = new TreeNode(fileName);

                //根节点下的子节点（文件夹）
                nodes.Nodes.Add(rootNode);

                //递归
                SetNode(info.FullName, rootNode);

                dictionaryList.Add(new Type(fileName));
            }

            //获取文件路径
            string[] fileList = Directory.GetFiles(fullRoad);
            for (int i = 0; i < fileList.Length; i++)
            {
                string description;
                string title;
                string fatherDirectory;
                string fileExtension;
                //可实例化的文件流操作
                FileInfo info = new FileInfo(fileList[i]);
                //文件名称
                string fileName = info.Name;
                TreeNode rootNode = new TreeNode(fileName);

                //根节点下的子节点（包含文件）
                nodes.Nodes.Add(rootNode);

                string Strpath = info.DirectoryName+"\\"+fileName;  //文本文件的全路径，用于打开输入流。
                byte[] buffer = File.ReadAllBytes(Strpath);  //以二进制方式读取文本文件并返回byte数组

                //获得文章内容
                string StrContent = Encoding.UTF8.GetString(buffer);//以UTF-8编码方式将二进制数组转换成string类型变量并返回

                //获取文章描述
                description = Regex.Replace(StrContent, "```([\\w\\W]*)```", "").Replace(" ", "").Replace("#", "").Replace("-", "").Replace("\n", "").Replace("\r", "").Replace("*", "");
                if (description.Length <= 150)
                {
                }
                else
                {
                    description= description.Substring(0, 149);
                }

                //获取文章标题
                fileExtension = info.Extension;
                title = fileName.Replace(fileExtension,"");

                //获取文章父分类
                fatherDirectory = info.DirectoryName;
                char[] sepatator = new char[1];
                sepatator[0] = '\\';
                string[] strList = fatherDirectory.Split(sepatator);
                string result = strList[strList.Length - 1];
                fatherDirectory = result;

                fileNameList.Add(new Blog(StrContent,File.GetCreationTime(Strpath),title,description,fatherDirectory,File.GetLastWriteTime(Strpath)));

            }
        }

        //上传type
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定上传目录？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK) {
                MySqlConnection mySqlConnection = MysqlConn.getConn();
                MySqlCommand mySqlCommand;
                String sql;
                try
                {
                    mySqlConnection.Open();
                    for (int i = 0; i < dictionaryList.Count; i++)
                    {

                        sql = "insert into t_type(name) values('"+ dictionaryList[i].Name + "');";
                        mySqlCommand = new MySqlCommand(sql, mySqlConnection);
                        if (mySqlCommand.ExecuteNonQuery() > 0)
                        {
                            
                        }

                    }
                    MessageBox.Show("上传成功");

                }
                catch(Exception ex)
                {
                    MessageBox.Show("上传失败:"+ex.Message);
                }
                finally
                {
                    MysqlConn.closeConn();
                }
            }
        }

        //清空type数据表
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定清空？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                MySqlConnection mySqlConnection = MysqlConn.getConn();
                MySqlCommand mySqlCommand;
                String sql;
                try
                {
                    mySqlConnection.Open();
                    sql = "SET FOREIGN_KEY_CHECKS=0";
                    mySqlCommand = new MySqlCommand(sql, mySqlConnection);
                    mySqlCommand.ExecuteNonQuery();
                    sql = "truncate table t_type";
                    mySqlCommand = new MySqlCommand(sql, mySqlConnection);
                    mySqlCommand.ExecuteNonQuery();
                    sql = "SET FOREIGN_KEY_CHECKS=1";
                    mySqlCommand = new MySqlCommand(sql, mySqlConnection);
                    mySqlCommand.ExecuteNonQuery();
                    MessageBox.Show("数据已经清除！");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("清空失败:" + ex.Message);
                }
                finally
                {
                    MysqlConn.closeConn();
                }
            }
        }

        //清空blog数据表
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定清空？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                MySqlConnection mySqlConnection = MysqlConn.getConn();
                MySqlCommand mySqlCommand;
                String sql;
                try
                {
                    mySqlConnection.Open();
                    sql = "SET FOREIGN_KEY_CHECKS=0";
                    mySqlCommand = new MySqlCommand(sql, mySqlConnection);
                    mySqlCommand.ExecuteNonQuery();
                    sql = "truncate table t_blog";
                    mySqlCommand = new MySqlCommand(sql, mySqlConnection);
                    mySqlCommand.ExecuteNonQuery();
                    sql = "SET FOREIGN_KEY_CHECKS=1";
                    mySqlCommand = new MySqlCommand(sql, mySqlConnection);
                    mySqlCommand.ExecuteNonQuery();
                    MessageBox.Show("数据已经清除！");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("清空失败:" + ex.Message);
                }
                finally
                {
                    MysqlConn.closeConn();
                }
            }
        }

        //上传blog数据表
        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确定上传blog？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                MySqlConnection mySqlConnection = MysqlConn.getConn();
                MySqlConnection mySqlConnectionRead = MysqlConn.getConnRead();
                MySqlCommand mySqlCommand;
                MySqlDataReader mySqlDataReader;
                String sql;
                int type_id;
                try
                {
                    mySqlConnection.Open();
                    for (int i = 0; i < fileNameList.Count; i++)
                    {
                        mySqlConnectionRead.Open();
                        sql = "select id from t_type where name='" + fileNameList[i].FatherDictionary + "'";
                        mySqlCommand = new MySqlCommand(sql, mySqlConnectionRead);
                        mySqlDataReader = mySqlCommand.ExecuteReader();
                        mySqlDataReader.Read();
                        type_id = mySqlDataReader.GetInt32(0);
                        MysqlConn.closeConnRead();

                        sql = "insert into t_blog(create_time,update_time, content, title,user_id,type_id,  description) " +
                            "VALUES (@Createtime,@Updatetime,@Content,@Title,1,@Type_id,@Description);";
                        mySqlCommand = new MySqlCommand(sql, mySqlConnection);
                        mySqlCommand.Prepare();

                        MySqlParameter p1 = new MySqlParameter("@Createtime", MySqlDbType.DateTime);
                        p1.Value = fileNameList[i].Createtime;
                        MySqlParameter p2 = new MySqlParameter("@Content", MySqlDbType.LongText);
                        p2.Value = fileNameList[i].Content;
                        MySqlParameter p3 = new MySqlParameter("@Title", MySqlDbType.VarChar);
                        p3.Value = fileNameList[i].Title;
                        MySqlParameter p4 = new MySqlParameter("@Type_id", MySqlDbType.Int64);
                        p4.Value = type_id;
                        MySqlParameter p5 = new MySqlParameter("@Description", MySqlDbType.VarChar);
                        MySqlParameter p6 = new MySqlParameter("@Updatetime", MySqlDbType.DateTime);
                        p6.Value = fileNameList[i].Updatetime;
                        p5.Value = fileNameList[i].Description;
                        mySqlCommand.Parameters.Add(p1);
                        mySqlCommand.Parameters.Add(p2);
                        mySqlCommand.Parameters.Add(p3);
                        mySqlCommand.Parameters.Add(p4);
                        mySqlCommand.Parameters.Add(p5);
                        mySqlCommand.Parameters.Add(p6);

                        if (mySqlCommand.ExecuteNonQuery() > 0)
                        {
                        }

                    }
                    MessageBox.Show("blog上传成功！");


                }
                catch (Exception ex)
                {
                    MessageBox.Show("上传失败:" + ex.Message);
                }
                finally
                {
                    MysqlConn.closeConn();
                }
            }
        }

    }
}
