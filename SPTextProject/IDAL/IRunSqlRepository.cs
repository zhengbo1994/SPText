using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace SPTextProject.IDAL
{
    public interface IRunSqlRepository
    {
        DataTable runSql(string sql, params object[] pamrs);

        bool ExecutePropToSLGAddOrDel(string sql, params SqlParameter[] parameters);

        bool ExecuteCommand(string sql, params SqlParameter[] parameters);
        bool ExecuteProp(string sql, params SqlParameter[] parameters);
        DataTable ExecutePropbyTable(string sql, params SqlParameter[] parameters);
        DataTable ExecutePropbyTableMDGLASS(string sql, params SqlParameter[] parameters);
        bool ExecuteProp(string sql, SqlConnection conn, SqlTransaction tran, SqlParameter[] parameters);
    }
}
