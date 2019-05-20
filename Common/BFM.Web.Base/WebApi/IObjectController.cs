using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BFM.Web.Base.WebApi
{
    public interface IObjectController<T> : IDisposable where T : class, new()
    {
        bool Add(T t);
        bool AddEntity(List<T> entitys);
        bool Update(T t);
        bool Delete(T t);
        bool Delete(List<T> entitys);
        bool Delete(Expression<Func<T, bool>> whereLambda);
        int GetCount(Expression<Func<T, bool>> WhereLambda);
        List<T> GetModels(Expression<Func<T, bool>> whereLambda);
        List<T> GetModelsByPage(int pageSize, int pageIndex, bool isAsc, string orderStr, Expression<Func<T, bool>> whereLambda);
        T GetFirstOrDefault(string keyvalue);
        T GetFirstOrDefault(Expression<Func<T, bool>> whereLambda);
    }
}
