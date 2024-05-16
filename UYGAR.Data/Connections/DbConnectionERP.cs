using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using UYGAR.Data.Base;

namespace UYGAR.Data.Connections
{
    public class DbConnectionERP : DbConnectionBase
    {
        public override string DbConnectionString => $"{ConfigurationManager.AppSettings["ConnErp"]}";
        public override DataTable ExecuteDataTable(string query)
        {
            try
            {
                using (var newconnection = new SqlConnection(DbConnectionString))
                {
                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();

                    using (var cmdS = new SqlCommand(query, newconnection))
                    {


                        DataTable table = new DataTable();
                        using (SqlDataAdapter adapterS = new SqlDataAdapter(cmdS))
                        {
                            adapterS.Fill(table);
                        }
                        newconnection.Close();
                        newconnection.Dispose();
                        return table;

                    }

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public override object ExecuteScalar(string query)
        {
            try
            {

                using (var newconnection = new SqlConnection(DbConnectionString))
                {
                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();
                    object returneddata = null;
                    using (var cmdS = new SqlCommand())
                    {

                        cmdS.CommandText = query;
                        cmdS.Connection = newconnection;
                        returneddata = cmdS.ExecuteScalar();
                    }
                    newconnection.Close();
                    newconnection.Dispose();
                    return returneddata;



                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public override int ExecuteScalar(string query, List<DbParameter> parametrs)
        {
            int retval = 0;
            try
            {

                using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
                {
                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();

                    using (SqlCommand cmdS = new SqlCommand(query, newconnection))
                    {
                        parametrs.ForEach(item => cmdS.Parameters.Add(item));
                        retval = cmdS.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return retval;
        }
        public override int ExecuteScalarInsert(string query, List<DbParameter> parameters)
        {
            int retval = -1;
            try
            {

                using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
                {

                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();

                    using (SqlCommand cmdS = new SqlCommand(query, newconnection))
                    {
                        parameters.ForEach(item => cmdS.Parameters.Add(item));
                        retval = Convert.ToInt32(cmdS.ExecuteScalar());

                    }
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return retval;
        }

        public override DataSet LoadToDataSet(string query)
        {
            try
            {
                DataSet set = new DataSet();
                using (SqlConnection newconnection = new SqlConnection(DbConnectionString))
                {
                    if (newconnection.State != ConnectionState.Open)
                        newconnection.Open();

                    using (SqlCommand cmdS = new SqlCommand(query, newconnection))
                    {
                        using (SqlDataAdapter adapet = new SqlDataAdapter(cmdS))
                        {
                            DataTable table = new DataTable();
                            adapet.Fill(table);
                            set.Tables.Add(table);
                        }
                    }
                }
                return set;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
