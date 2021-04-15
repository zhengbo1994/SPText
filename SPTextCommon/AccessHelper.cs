using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextCommon
{
    public class AccessHelper
    {
        string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data source=" + System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\LensCode.mdb;Persist Security Info=False;Jet OLEDB:Database Password=";

        public DataSet Query(string sql)
        {
            DataSet _ds = new DataSet();
            OleDbConnection _conn = new OleDbConnection(this.connectionString);
            try
            {

                _conn.Open();
                OleDbDataAdapter _da = new OleDbDataAdapter(sql, _conn);
                _da.Fill(_ds, "ds");

            }
            catch (OleDbException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _conn.Close();
            }
            return _ds;
        }

    }
}
