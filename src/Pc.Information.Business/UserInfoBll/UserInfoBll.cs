﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pc.Information.DataAccess.UserInfoDataAccess;
using Pc.Information.Interface.IUserInfoBll;
using Pc.Information.Model.User;

namespace Pc.Information.Business.UserInfoBll
{
    /// <summary>
    /// User info business deal.
    /// </summary>
    public class UserInfoBll: IUserInfoBll
    {
        /// <summary>
        /// Test MySqlhelper method.
        /// </summary>
        public void GetUserInfoT()
        {
            
        }

        /// <summary>
        /// Get user info by username and password.
        /// </summary>
        /// <param name="username">login user name</param>
        /// <param name="password">login for secret</param>
        /// <returns></returns>
        public Task GetUserInfo(string username, string password)
        {
            var info = Task.Factory.StartNew(GetUserInfoT);
            return info;
        }

        IList<PiFUsersModel> IUserInfoBll.GetUserInfo()
        {
            var userInfoDataAccess = new UserInfoDataAccess();
            var useElist=userInfoDataAccess.GetUserInfo();
            return useElist;
        }
    }
}
