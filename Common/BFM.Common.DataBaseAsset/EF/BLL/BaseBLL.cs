using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BFM.Common.DataBaseAsset.EF
{
    public abstract class BaseBLL<T> : IDisposable where T : class, new()
    {
        public IBaseDAL<T> Dal { get; set; }
        public abstract void SetDal();

        public BaseBLL()
        {
            SetDal();
        }

        public bool Add(T t)
        {
            Dal.Add(t);
            return Dal.SaveChanges();
        }

        public bool AddEntity(List<T> entitys)
        {
            return Dal.AddEntity(entitys);
        }

        public bool Update(T t)
        {
            Dal.Update(t);
            return Dal.SaveChanges();
        }

        public bool Delete(T t)
        {
            Dal.Delete(t);
            return Dal.SaveChanges();
        }

        public bool Delete(List<T> entitys)
        {
            Dal.Delete(entitys);
            return Dal.SaveChanges();
        }

        public bool Delete(Expression<Func<T, bool>> whereLambda)
        {
            Dal.Delete(whereLambda);
            return Dal.SaveChanges();
        }

        public List<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
			return Dal.GetModels(whereLambda).ToList();
        }

        public int GetCount(Expression<Func<T, bool>> whereLambda)
        {
			return Dal.GetCount(whereLambda);
        }

        public List<T> GetModelsByPage(int pageSize, int pageIndex, bool isAsc, string orderStr, Expression<Func<T, bool>> whereLambda)
        {
			return Dal.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLambda).ToList();
        }

        public T GetFirstOrDefault(string keyvalue)
        {
            return Dal.GetFirstOrDefault(keyvalue);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> whereLambda)
        {
            return Dal.GetFirstOrDefault(whereLambda);
        }

        public void Dispose()
        {
            //todo 暂时释放
            //Dal.Dispose();
            //Dal = null;
        }
    }
}
