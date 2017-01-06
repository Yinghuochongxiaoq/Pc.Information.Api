﻿using System;
using System.Text;
using Dapper;
using MySql.Data.MySqlClient;

namespace Pc.Information.DataAccess.FreshSqlHelper
{
    public class FreshSqlHelper
    {
        public static void tst()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            MySqlConnection con = new MySqlConnection("server=127.0.0.1;database=test;uid=root;pwd=***;charset='gbk';SslMode=None");
            //新增数据
            con.Execute("insert into user values(null, '测试', 'http://www.cnblogs.com/linezero/', 18)");
            //新增数据返回自增id
            var id = con.QueryFirst<int>("insert into user values(null, 'linezero', 'http://www.cnblogs.com/linezero/', 18);select last_insert_id();");
            //修改数据
            con.Execute("update user set UserName = 'linezero123' where Id = @Id", new { Id = id });
            //查询数据
            var list = con.Query<User>("select * from user");
            foreach (var item in list)
            {
                Console.WriteLine($"用户名：{item.UserName} 链接：{item.Url}");
            }
            //删除数据
            con.Execute("delete from user where Id = @Id", new { Id = id });
            Console.WriteLine("删除数据后的结果");
            list = con.Query<User>("select * from user");
            foreach (var item in list)
            {
                Console.WriteLine($"用户名：{item.UserName} 链接：{item.Url}");
            }
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public int Age { get; set; }
    }
}
