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

        
        [System.Web.Http.HttpGet]
        public Result deleteCategory(int id)
        {
            Result obj = new Result();
            try
            {
                var re = Request;
                var headers = re.Headers;
                bool status = false;
                string token = "";
                if (headers.Contains("Authorization"))
                {
                    token = headers.GetValues("Authorization").First();
                    status = checkToken(token);
                }
                if (status)
                {
                    Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                    cmdParameters["state"] = new SqlParameter("state", "delete");
                    cmdParameters["id"] = new SqlParameter("id", id);
                    DBUtility utl = new DBUtility();
                    int result = utl.ExecuteCommand("USPCategory", cmdParameters);
                    if (result == 1)
                    {
                        obj.status = "deleted";
                    }
                    else
                    {
                        obj.status = "error";
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return obj;
        }

        
        [System.Web.Http.HttpGet]
        public Category getCategoryByID(int id)
        {
            try
            {

                Category obj = new Category();
                var re = Request;
                var headers = re.Headers;
                bool status = false;
                string token = "";
                if (headers.Contains("Authorization"))
                {
                    token = headers.GetValues("Authorization").First();
                    status = checkToken(token);
                }
                if (status)
                {

                    Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                    cmdParameters["state"] = new SqlParameter("state", "edit");
                    cmdParameters["id"] = new SqlParameter("id", id);
                    DBUtility utl = new DBUtility();
                    DataTable dt = utl.getDataTabe("USPCategory", cmdParameters);
                    if (dt.Rows.Count > 0)
                    {

                        obj.id = Convert.ToInt32(dt.Rows[0]["id"]);
                        obj.category_name = Convert.ToString(dt.Rows[0]["category_name"]);
                        obj.created_at = Convert.ToDateTime(dt.Rows[0]["created_at"]);
                        obj.is_active = Convert.ToBoolean(dt.Rows[0]["is_active"]);//mmmmmmmmmm
                    }
                }

                return obj;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        

        [System.Web.Http.HttpPost]
        public Result createUpdateCategory()
        {
            Result obj = new Result();
            try
            {
                var re = Request;
                var headers = re.Headers;
                bool status = false;
                string token = "";
                if (headers.Contains("Authorization"))
                {
                    token = headers.GetValues("Authorization").First();
                    status = checkToken(token);
                }
                if (status)
                {
                    var httpRequest = HttpContext.Current.Request;
                    string postid = httpRequest["id"];

                    Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                    cmdParameters["category_name"] = new SqlParameter("category_name", httpRequest["category_name"]);
                    cmdParameters["is_active"] = new SqlParameter("is_active", httpRequest["is_active"]);
                    if (postid != null)
                    {
                        cmdParameters["state"] = new SqlParameter("state", "update");
                        cmdParameters["id"] = new SqlParameter("id", postid);
                    }
                    else
                    {
                        cmdParameters["state"] = new SqlParameter("state", "insert");
                    }
                    DBUtility utl = new DBUtility();
                    int result = utl.ExecuteCommand("USPCategory", cmdParameters);
                    if (result == 1)
                    {
                        obj.status = "Created or Updated";
                    }
                    else
                    {
                        obj.status = "error";
                    }
                }
                else
                {
                    obj.status = "error";
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return obj;
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
