using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.DataHelper.Data
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> ListAll();
        IEnumerable<TEntity> ListById(int id);

        IEnumerable<TEntity> ListByCustom(string fieldName, string fieldValue, CompareType compareType = CompareType.Equal);
        IEnumerable<TEntity> ListByCustoms(Dictionary<string, string> keyValuePairs);
        IEnumerable<TEntity> ListByCustoms(Dictionary<string, ListByCustomsEntity> keyValuePairs);

        IEnumerable<TEntity> ListByCustomWhereIn(string fieldName, params string[] fieldValues);

        int Insert(TEntity entity);
        object InsertBackRecord(TEntity entity);

        int Update(TEntity entity);

        int Delete(TEntity entity);
        int Delete(int id);

        int DeleteByCustomer(string fieldName, string fieldValue);
    }

    public class BaseEntity
    {
        public int Id { get; set; }
    }

    public class ListByCustomsEntity
    {
        public string Value { get; set; }
        public bool IsEq { get; set; }
        public bool IsAnd { get; set; }
    }

    public enum CompareType
    {
        Equal = 0,
        Unequal = 1,
        greaterThan = 2,
        lessThan = 3
    }
}
