using deepandaniAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace deepandaniAPI.Controllers
{
    public class CategoryController : ApiController
    {
        [System.Web.Http.HttpGet]
        public List<Category> listOfCategories()
        {
            try
            {
                //var re = Request;
                //var headers = re.Headers;
                var request = HttpContext.Current.Request;
                var authHeader = request.Headers["Authorization"];
                bool status = false;
                string token = "";
                if (authHeader != null)
                {
                    token = Convert.ToString(authHeader);
                    status = checkToken(token);
                }
                if (status)
                {
                    List<Category> objlist = new List<Category>();

                    Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                    cmdParameters["state"] = new SqlParameter("state", "select");
                    DBUtility utl = new DBUtility();
                    DataTable dt = utl.getDataTabe("USPCategory", cmdParameters);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Category obj = new Category();
                        obj.id = Convert.ToInt32(dt.Rows[i]["id"]);
                        obj.category_name = Convert.ToString(dt.Rows[i]["category_name"]);
                        obj.is_active = Convert.ToBoolean(dt.Rows[i]["is_active"]);
                        obj.created_at = Convert.ToDateTime(dt.Rows[i]["created_at"]);
                        objlist.Add(obj);
                    }

                    return objlist;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool checkToken(string token)
        {
            try
            {
                Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                cmdParameters["state"] = new SqlParameter("state", "checktoken");
                cmdParameters["token"] = new SqlParameter("token", token);
                DBUtility utl = new DBUtility();
                string status = utl.ExecuteScalar("SPLogin", cmdParameters);
                if (status == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
