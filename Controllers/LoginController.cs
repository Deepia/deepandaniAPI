using deepandaniAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace deepandaniAPI.Controllers
{
    public class LoginController : ApiController
    {
        [HttpGet]
        public User checkLogin(string id, string id1)
        {
            User obj = new User();
            try
            {
                string username = id;
                string password = id1;
                Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                cmdParameters["username"] = new SqlParameter("username", username);
                cmdParameters["password"] = new SqlParameter("password", password);
                cmdParameters["state"] = new SqlParameter("state", "login");
                DBUtility utl = new DBUtility();
                string token = utl.ExecuteScalar("SPLogin", cmdParameters);
                if(token!="")
                {
                    obj.username = username;
                    obj.password = password;
                    obj.token = token;
                }
                return obj;
            }
            catch(Exception ex)
            {
                return obj;
            }
        }
      }
}
