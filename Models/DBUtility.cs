using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace deepandaniAPI.Models
{
    public class DBUtility
    {
        public SqlConnection GetConnection(string connectionName= "DefaultConnection")
        {
            string cnstr = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            SqlConnection cn = new SqlConnection(cnstr);
            cn.Open();
            return cn;
        }

        public DataSet getDataSet(string storedProcName,Dictionary<string,SqlParameter>procParameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection cn = GetConnection())
            {
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcName;
                    // assign parameters passed in to the command
                    foreach (var procParameter in procParameters)
                    {
                        cmd.Parameters.Add(procParameter.Value);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }
            }
            return ds;
        }

        public DataTable getDataTabe(string storedProcName, Dictionary<string, SqlParameter> procParameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = GetConnection())
            {
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcName;
                    // assign parameters passed in to the command
                    foreach (var procParameter in procParameters)
                    {
                        cmd.Parameters.Add(procParameter.Value);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public int ExecuteCommand(string storedProcName,Dictionary<string, SqlParameter> procParameters)
        {
            int rc;
            using (SqlConnection cn = GetConnection())
            {
                // create a SQL command to execute the stored procedure
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcName;
                    // assign parameters passed in to the command
                    foreach (var procParameter in procParameters)
                    {
                        cmd.Parameters.Add(procParameter.Value);
                    }
                    rc = cmd.ExecuteNonQuery();
                }
            }
            return rc;
        }

        public string ExecuteScalar(string storedProcName, Dictionary<string, SqlParameter> procParameters)
        {
            
            using (SqlConnection cn = GetConnection())
            {
                // create a SQL command to execute the stored procedure
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcName;
                    // assign parameters passed in to the command
                    foreach (var procParameter in procParameters)
                    {
                        cmd.Parameters.Add(procParameter.Value);
                    }
                    return cmd.ExecuteScalar().ToString();
                }
            }
        }

    }
}