using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BFM.Common.DataBaseAsset.EF
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IBaseDAL<T> where T : class, new()
    {
        void Add(T t);
        bool AddEntity(List<T> entitys);
        void Update(T t);
        void Delete(T t);
        void Delete(List<T> entitys);
        void Delete(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda);
        int GetCount(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> GetModelsByPage(int pageSize, int pageIndex, bool isAsc, string orderStr, Expression<Func<T, bool>> whereLambda);
        bool SaveChanges();
        T GetFirstOrDefault(string keyvalue);
        T GetFirstOrDefault(Expression<Func<T, bool>> whereLambda);
        void Dispose();
    }
}
