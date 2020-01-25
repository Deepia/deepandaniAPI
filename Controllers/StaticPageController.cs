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
    public class StaticPageController : ApiController
    {
        [HttpGet]
        public Page getPage(string id)
        {
            try
            {

                Page obj = new Page();
                Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                cmdParameters["slug"] = new SqlParameter("slug", id);
                cmdParameters["state"] = new SqlParameter("state", "getpage");
                DBUtility utl = new DBUtility();
                DataTable dt = utl.getDataTabe("sppages", cmdParameters);
                if (dt.Rows.Count > 0)
                {
                    obj.id = Convert.ToInt32(dt.Rows[0]["id"]);
                    obj.title = Convert.ToString(dt.Rows[0]["title"]);
                    obj.description = Convert.ToString(dt.Rows[0]["descriptions"]);
                }
                return obj;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        


        
        [HttpPost]
        public Contact contactForm(Contact obj)
        {
            try
            {
                Dictionary<string, SqlParameter> cmdParameters = new Dictionary<string, SqlParameter>();
                cmdParameters["name"] = new SqlParameter("name", obj.name);
                cmdParameters["email"] = new SqlParameter("email", obj.email);
                cmdParameters["phone"] = new SqlParameter("phone", obj.phone);
                cmdParameters["message"] = new SqlParameter("message", obj.message);
                DBUtility utl = new DBUtility();
                int res = utl.ExecuteCommand("SPcontact", cmdParameters);
                if(res>0)
                {
                    return obj;
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
    }
}
