using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;


namespace SPText.Common.DataHelper.Data
{
    public partial class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity//,new()
    {
        public SqlHelperBase SqlHelperBase { get; set; }

        public Repository(string connStr)
        {
            SqlHelperBase = new SqlHelperBase(connStr);
        }


        public IEnumerable<TEntity> ListAll()
        {
            DataTable dt = SqlHelperBase.ExecuteTable(_listAllText);
            ToModelList(dt, out List<TEntity> list);
            return list;
        }
        public IEnumerable<TEntity> ListById(int id)
        {
            DataTable dt = SqlHelperBase.ExecuteTable(_listByIdText,
                new SqlParameter("@Id", id));
            ToModelList(dt, out List<TEntity> list);
            return list;
        }

        public IEnumerable<TEntity> ListByCustom(
            string fieldName,
            string fieldValue,
            CompareType compareType = CompareType.Equal)
        {
            string eq = GetCompare(compareType);
            string strCols = GetColumnsStr();
            DataTable dt =
                SqlHelperBase.ExecuteTable(
                    $"{_listAllText} WHERE {fieldName} {eq} @fieldName",
                    new SqlParameter("@fieldName", fieldValue));
            ToModelList(dt, out List<TEntity> list);
            return list;
        }
        public IEnumerable<TEntity> ListByCustoms(Dictionary<string, string> keyValuePairs)
        {
            //放置搜索语句
            List<string> whereValue = new List<string>();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            foreach (var keyValuePair in keyValuePairs)
            {
                whereValue.Add($"{keyValuePair.Key} = @{keyValuePair.Key}");
                sqlParameters.Add(new SqlParameter($"@{keyValuePair.Key}", keyValuePair.Value));
            }
            string strCols = GetColumnsStr();
            DataTable dt = SqlHelperBase.ExecuteTable(
                $"{_listAllText} WHERE {string.Join(" AND ", whereValue)}",
                sqlParameters.ToArray());
            ToModelList(dt, out List<TEntity> list);
            return list;
        }
        public IEnumerable<TEntity> ListByCustoms(Dictionary<string, ListByCustomsEntity> keyValuePairs)
        {
            //放置搜索语句
            List<string> whereValue = new List<string>();
            //放置参数
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            string whereText = "";
            int i = 0;
            foreach (var keyValuePair in keyValuePairs)
            {
                var value = keyValuePair.Value;
                string eq = value.IsEq ? "=" : "<>"; //判断是等于或不等于
                string andText = value.IsAnd ? "AND" : "OR"; // 是否为AND或OR连接
                whereText += $"{(i++ == 0 ? "" : " " + andText)} {keyValuePair.Key} {eq} @{keyValuePair.Key}"; // 拼接where语句
                sqlParameters.Add(new SqlParameter($"@{keyValuePair.Key}", value.Value)); //添加筛选参数
            }

            string strCols = GetColumnsStr();
            DataTable dt = SqlHelperBase.ExecuteTable(
                $"{_listAllText} WHERE {whereText}",
                sqlParameters.ToArray());
            ToModelList(dt, out List<TEntity> list);
            return list;
        }

        public IEnumerable<TEntity> ListByCustomWhereIn(string fieldName, params string[] fieldValues)
        {
            if (fieldValues == null || fieldValues.Length == 0) return ListAll();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            List<string> sqlParams = new List<string>();
            for (int i = 0; i < fieldValues.Length; i++)
            {
                sqlParams.Add("@field" + i);
                sqlParameters.Add(new SqlParameter("@field" + i, fieldValues[i]));
            }
            string strCols = GetColumnsStr();
            DataTable dt = SqlHelperBase.ExecuteTable(
                $"{_listAllText} WHERE {fieldName} IN({string.Join(",", sqlParams)})",
                sqlParameters.ToArray());
            ToModelList(dt, out List<TEntity> list);
            return list;
        }

        public int Insert(TEntity entity)
        {
            IEnumerable<PropertyInfo> props = _type.GetProperties().Where(m => !m.Name.Equals("Id"));

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            foreach (var prop in props)
            {
                sqlParameters.Add(new SqlParameter($"@{prop.Name}", SqlHelperBase.ToDbValue(prop.GetValue(entity))));
            }

            return SqlHelperBase.ExecuteNoQuery(_insertText, sqlParameters.ToArray());
        }
        public object InsertBackRecord(TEntity entity)
        {
            var props = _type.GetProperties().Where(m => !m.Name.Equals("Id"));
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            foreach (var prop in props)
            {
                sqlParameters.Add(new SqlParameter($"@{prop.Name}", SqlHelperBase.ToDbValue(prop.GetValue(entity))));
            }

            return SqlHelperBase.ExecuteScalar(_insertBackText, sqlParameters.ToArray());
        }

        public int Update(TEntity entity)
        {
            var props = _type.GetProperties();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            foreach (var prop in props)
            {
                sqlParameters.Add(new SqlParameter($"@{prop.Name}", SqlHelperBase.ToDbValue(prop.GetValue(entity))));
            }

            return SqlHelperBase.ExecuteNoQuery(_updateText, sqlParameters.ToArray());
        }

        public int Delete(TEntity entity)
        {
            string sqlText = $"{_deleteText} WHERE Id = @Id";
            return SqlHelperBase.ExecuteNoQuery(sqlText, new SqlParameter("@Id", _type.GetProperty("Id").GetValue(entity)));
        }
        public int Delete(int id)
        {
            string sqlText = $"{_deleteText} WHERE Id = @Id";
            return SqlHelperBase.ExecuteNoQuery(sqlText, new SqlParameter("@Id", id));
        }

        public int DeleteByCustomer(string fieldName, string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue)) return 0;
            string sqlText = $"{_deleteText} WHERE {fieldName} = @{fieldName}";
            return SqlHelperBase.ExecuteNoQuery(sqlText, new SqlParameter($"@{fieldName}", fieldValue));
        }

        private static void ToModelList(DataTable dt, out List<TEntity> list)
        {
            list = new List<TEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(ToModel(dr));
            }
        }

        private static TEntity ToModel(DataRow dr)
        {
            //如果给泛型加了一个new()的约束，那么次泛型可以直接通过new方法实例化
            //TEntity entity = new TEntity();
            //如果没有给泛型加new()的约束，则需要通过反射生成实例
            TEntity entity = (TEntity)Activator.CreateInstance(_type);
            var props = _type.GetProperties();
            foreach (var prop in props)
            {
                if (dr.Table.Columns.Contains(prop.Name))
                {
                    prop.SetValue(entity, SqlHelperBase.FromDbValue(dr[prop.Name]));
                }
            }
            return entity;
        }

        private static string GetColumnsStr()
        {
            var cols = GetColumns();
            string strCols = string.Join(",", cols);
            return strCols;
        }
        private static IEnumerable<string> GetColumns()
        {
            var cols = _props.Select(m => m.GetColumnName());
            return cols;
        }

        private string GetCompare(CompareType compareType)
        {
            switch (compareType)
            {
                default:
                case CompareType.Equal:
                    return "=";
                case CompareType.Unequal:
                    return "<>";
                case CompareType.greaterThan:
                    return ">";
                case CompareType.lessThan:
                    return "<";
            }
        }
    }

    public partial class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        static Repository()
        {
            _type = typeof(TEntity);
            _props = _type.GetProperties();
            string strCols = GetColumnsStr();
            var cols = GetColumns().Where(m => !m.Equals("Id"));
            string strColsWithoutId = string.Join(",", cols);
            //listAll
            _listAllText = $"SELECT {strCols} FROM {_type.GetTableName()}";
            //listById
            _listByIdText = $@"SELECT {strCols} FROM {_type.GetTableName()} WHERE Id = @Id";
            //insert
            _insertText =
                $"INSERT INTO {_type.GetTableName()} ({string.Join(",", strColsWithoutId)}) VALUES ({string.Join(",", cols.Select(m => "@" + m))})";
            //insertBackRecord
            _insertBackText =
                $"INSERT INTO {_type.GetTableName()} ({string.Join(",", strColsWithoutId)}) output inserted.Id VALUES ({string.Join(",", cols.Select(m => "@" + m))})";
            //update
            List<string> list = new List<string>();
            foreach (var prop in _props)
            {
                if (prop.GetColumnName() != "Id")
                {
                    list.Add($"{prop.GetColumnName()} = @{prop.GetColumnName()}");
                }
            }

            string updateItem = string.Join(",", list);
            _updateText = $"UPDATE {_type.GetTableName()} SET {updateItem} WHERE Id=@Id";

            //delete
            _deleteText = $"DELETE FROM {_type.GetTableName()}";
        }

        private static Type _type;
        private static PropertyInfo[] _props;
        private static string _listAllText;
        private static string _listByIdText;
        private static string _insertText;
        private static string _insertBackText;
        private static string _updateText;
        private static string _deleteText;
    }
}
