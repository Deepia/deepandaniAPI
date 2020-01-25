using deepandaniAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace deepandaniAPI.Controllers
{
    
    public class BlogController : ApiController
    {
        
        [System.Web.Http.HttpGet]
        public List<Blog> listblogs()
        {
            try
            {

                List<Blog> objlist = new List<Blog>();
                Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                cmdParameters["state"] = new SqlParameter("state", "list");
                DBUtility utl = new DBUtility();
                DataTable dt=utl.getDataTabe("USPblogs", cmdParameters);
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    Blog obj = new Blog();
                    obj.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    obj.title = Convert.ToString(dt.Rows[i]["title"]);
                    obj.author= Convert.ToString(dt.Rows[i]["author"]);
                    obj.short_desc = Convert.ToString(dt.Rows[i]["short_desc"]);
                    obj.image = Convert.ToString(dt.Rows[i]["imageName"]);
                    obj.created_at = Convert.ToDateTime(dt.Rows[i]["created_at"]);
                    objlist.Add(obj);
                }
                return objlist;

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        [System.Web.Http.HttpGet]
        public List<Blog> listadminblogs()
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
                    status=checkToken(token);
                }
                if (status)
                {
                    List<Blog> objlist = new List<Blog>();

                    Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                    cmdParameters["state"] = new SqlParameter("state", "adminlist");
                    cmdParameters["token"] = new SqlParameter("token", token);
                    DBUtility utl = new DBUtility();
                    DataTable dt = utl.getDataTabe("USPblogs", cmdParameters);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Blog obj = new Blog();
                        obj.id = Convert.ToInt32(dt.Rows[i]["id"]);
                        obj.title = Convert.ToString(dt.Rows[i]["title"]);
                        obj.author = Convert.ToString(dt.Rows[i]["author"]);
                        obj.short_desc = Convert.ToString(dt.Rows[i]["short_desc"]);
                        obj.image = Convert.ToString(dt.Rows[i]["imageName"]);
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


        [System.Web.Http.HttpPost]
        [ValidateInput(false)]
        public Result createUpdateBlog()
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
                    string imageName = null;
                    var httpRequest = HttpContext.Current.Request;
                    var postedFile = httpRequest.Files["Image"];
                    string postid = httpRequest["id"];
                    if (postedFile != null)
                    {
                        imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                        imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                        var filePath = HttpContext.Current.Server.MapPath("~/images/" + imageName);
                        postedFile.SaveAs(filePath);
                    }
                    else if (postedFile == null && postid != null)
                    {
                        imageName = getimageName(postid);
                    }

                    Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                    cmdParameters["title"] = new SqlParameter("title", httpRequest["title"]);
                    cmdParameters["descriptions"] = new SqlParameter("descriptions", httpRequest["description"]);
                    cmdParameters["imageName"] = new SqlParameter("imageName", imageName);
                    cmdParameters["is_featured"] = new SqlParameter("is_featured", httpRequest["is_featured"]);
                    cmdParameters["is_active"] = new SqlParameter("is_active", httpRequest["is_active"]);
                    cmdParameters["short_desc"] = new SqlParameter("short_desc", httpRequest["short_desc"]);
                    cmdParameters["token"] = new SqlParameter("token", token);
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
                    int result = utl.ExecuteCommand("USPblogs", cmdParameters);
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


        [System.Web.Http.HttpPost]
        [ValidateInput(false)]
        public Result createUpdateStaticPage()
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
                    cmdParameters["title"] = new SqlParameter("title", httpRequest["title"]);
                    cmdParameters["descriptions"] = new SqlParameter("descriptions", httpRequest["description"]);
                    cmdParameters["is_active"] = new SqlParameter("is_active", httpRequest["is_active"]);
                    cmdParameters["slug"] = new SqlParameter("slug", httpRequest["slug"]);
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
                    int result = utl.ExecuteCommand("sppages", cmdParameters);
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

        [System.Web.Http.HttpGet]
        public Result deleteBlog(int id)
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
                    int result = utl.ExecuteCommand("USPblogs", cmdParameters);
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
        public Result deleteStaticPage(int id)
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
                    int result = utl.ExecuteCommand("sppages", cmdParameters);
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

        public string getimageName(string id)
        {
            try
            {
                Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                cmdParameters["state"] = new SqlParameter("state", "getimagename");
                cmdParameters["id"] = new SqlParameter("id", id);
                DBUtility utl = new DBUtility();
                string imageName = utl.ExecuteScalar("USPblogs", cmdParameters);
                return imageName;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [System.Web.Http.HttpGet]
        public List<Pages> getAllStaticPage()
        {
            try
            {

                List<Pages> objList = new List<Pages>();
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
                    cmdParameters["state"] = new SqlParameter("state", "select");
                    DBUtility utl = new DBUtility();
                    DataTable dt = utl.getDataTabe("sppages", cmdParameters);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Pages obj = new Pages();
                        obj.id = Convert.ToInt32(dt.Rows[i]["id"]);
                        obj.title = Convert.ToString(dt.Rows[i]["title"]);
                        //obj.description = Convert.ToString(dt.Rows[i]["descriptions"]);
                        obj.slug = Convert.ToString(dt.Rows[i]["slug"]);
                        obj.created_at = Convert.ToDateTime(dt.Rows[i]["created_at"]);
                        obj.is_active = Convert.ToBoolean(dt.Rows[i]["is_active"]);
                        objList.Add(obj);
                    }
                }

                return objList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [System.Web.Http.HttpGet]
        public Pages getStaticPageByID(int id)
        {
            try
            {

                Pages obj = new Pages();
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
                    DataTable dt = utl.getDataTabe("sppages", cmdParameters);
                    if(dt.Rows.Count>0)
                    {
                        
                        obj.id = Convert.ToInt32(dt.Rows[0]["id"]);
                        obj.title = Convert.ToString(dt.Rows[0]["title"]);
                        obj.description = Convert.ToString(dt.Rows[0]["descriptions"]);
                        obj.slug = Convert.ToString(dt.Rows[0]["slug"]);
                        obj.created_at = Convert.ToDateTime(dt.Rows[0]["created_at"]);
                        obj.is_active = Convert.ToBoolean(dt.Rows[0]["is_active"]);
                    }
                }

                return obj;

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
                string status=utl.ExecuteScalar("SPLogin", cmdParameters);
                if(status=="1")
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

        [System.Web.Http.HttpGet]
        public Blog blogDetail(int id)
        {
            try
            {

                Blog obj = new Blog();
                Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                cmdParameters["state"] = new SqlParameter("state", "detail");
                cmdParameters["id"] = new SqlParameter("id", id);
                DBUtility utl = new DBUtility();
                DataTable dt = utl.getDataTabe("USPblogs", cmdParameters);
                if (dt.Rows.Count>0)
                {
                    obj.id = Convert.ToInt32(dt.Rows[0]["id"]);
                    obj.title = Convert.ToString(dt.Rows[0]["title"]);
                    obj.author = Convert.ToString(dt.Rows[0]["author"]);
                    obj.short_desc = Convert.ToString(dt.Rows[0]["short_desc"]);
                    obj.image = Convert.ToString(dt.Rows[0]["imageName"]);
                    obj.created_at = Convert.ToDateTime(dt.Rows[0]["created_at"]);
                    obj.description= Convert.ToString(dt.Rows[0]["descriptions"]);
                }
                return obj;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [System.Web.Http.HttpGet]
        public BlogForAdmin getBlogForEdit(int id)
        {
            try
            {

                BlogForAdmin obj = new BlogForAdmin();
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
                    cmdParameters["state"] = new SqlParameter("state", "detail");
                    cmdParameters["id"] = new SqlParameter("id", id);
                    DBUtility utl = new DBUtility();
                    DataTable dt = utl.getDataTabe("USPblogs", cmdParameters);
                    if (dt.Rows.Count > 0)
                    {
                        obj.id = Convert.ToInt32(dt.Rows[0]["id"]);
                        obj.title = Convert.ToString(dt.Rows[0]["title"]);
                        obj.is_active = Convert.ToBoolean(dt.Rows[0]["is_active"]);
                        obj.is_featured = Convert.ToBoolean(dt.Rows[0]["is_featured"]);
                        obj.short_desc = Convert.ToString(dt.Rows[0]["short_desc"]);
                        obj.image = Convert.ToString(dt.Rows[0]["imageName"]);
                        obj.created_at = Convert.ToDateTime(dt.Rows[0]["created_at"]);
                        obj.description = Convert.ToString(dt.Rows[0]["descriptions"]);
                        obj.user_id= Convert.ToInt32(dt.Rows[0]["users_id"]);
                    }
                }
                return obj;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [System.Web.Http.HttpGet]
        public List<Blog> getRecentBlogs()
        {
            try
            {

                List<Blog> objlist = new List<Blog>();
                Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                cmdParameters["state"] = new SqlParameter("state", "recentblog");
                DBUtility utl = new DBUtility();
                DataTable dt = utl.getDataTabe("USPblogs", cmdParameters);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Blog obj = new Blog();
                    obj.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    obj.title = Convert.ToString(dt.Rows[i]["title"]);
                    obj.author = Convert.ToString(dt.Rows[i]["author"]);
                    obj.short_desc = Convert.ToString(dt.Rows[i]["short_desc"]);
                    obj.image = Convert.ToString(dt.Rows[i]["imageName"]);
                    obj.created_at = Convert.ToDateTime(dt.Rows[i]["created_at"]);
                    objlist.Add(obj);
                }
                return objlist;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [System.Web.Http.HttpGet]
        public List<category> getCategories()
        {
            try
            {

                List<category> objlist = new List<category>();
                Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                DBUtility utl = new DBUtility();
                DataTable dt = utl.getDataTabe("spcategories", cmdParameters);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    category obj = new category();
                    obj.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    obj.name = Convert.ToString(dt.Rows[i]["category_name"]);
                    objlist.Add(obj);
                }
                return objlist;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [System.Web.Http.HttpGet]
        public List<Blog> featuredblogs()
        {
            try
            {

                List<Blog> objlist = new List<Blog>();
                Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                cmdParameters["state"] = new SqlParameter("state", "featured");
                DBUtility utl = new DBUtility();
                DataTable dt = utl.getDataTabe("USPblogs", cmdParameters);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Blog obj = new Blog();
                    obj.id = Convert.ToInt32(dt.Rows[i]["id"]);
                    obj.title = Convert.ToString(dt.Rows[i]["title"]);
                    obj.author = Convert.ToString(dt.Rows[i]["author"]);
                    obj.short_desc = Convert.ToString(dt.Rows[i]["short_desc"]);
                    obj.image = Convert.ToString(dt.Rows[i]["imageName"]);
                    obj.created_at = Convert.ToDateTime(dt.Rows[i]["created_at"]);
                    objlist.Add(obj);
                }
                return objlist;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
