using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using BFM.Common.Base.Log;

namespace BFM.Common.DataBaseAsset.EF
{
    public class BaseDAL<T>: IBaseDAL<T> where T : class, new()
    {
        public DB_Service dbWrite = DBContextFactory.GetService();  //用于写的EF上下文，与读取的分开

        protected DB_Service dbRead => DBContextFactory.GetService();  //用于读取的EF上下文，每次新建一个

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="t"></param>
        public void Add(T t)
        {
            dbWrite.Set<T>().Add(t);
        }

        /// <summary>
        /// 同时增加多条数据到一张表（事务处理）
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool AddEntity(List<T> entitys)
        {
            foreach (var entity in entitys)
            {
                dbWrite.Entry<T>(entity).State = EntityState.Added;
            }
            // entitys.ForEach(c=>db.Entry<T>(c).State = EntityState.Added);//等价于上面的循环
            return SaveChanges();
        }

        public void Update(T t)
        {
            dbWrite.Set<T>().AddOrUpdate(t);
        }

        public void Delete(T t)
        {
            dbWrite = DBContextFactory.GetService();
            dbWrite.Set<T>().Attach(t);
            dbWrite.Set<T>().Remove(t);
        }
        public void Delete(List<T> entitys)
        {
            dbWrite = DBContextFactory.GetService();
            foreach (var t in entitys)
            {
                dbWrite.Set<T>().Attach(t);
                dbWrite.Set<T>().Remove(t);
            }
        }

        public void Delete(Expression<Func<T, bool>> whereLambda)
        {
            dbWrite = DBContextFactory.GetService();
            var entitys = dbWrite.Set<T>().Where(whereLambda).ToList();
            foreach (var t in entitys)
            {
                dbWrite.Set<T>().Attach(t);
                dbWrite.Set<T>().Remove(t);
            }
        }

        public IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return dbRead.Set<T>().Where(whereLambda);
        }

        public int GetCount(Expression<Func<T, bool>> whereLambda)
        {
            return dbRead.Set<T>().Where(whereLambda).Count();
        }

        public IQueryable<T> GetModelsByPage(int pageSize, int pageIndex, bool isAsc, string orderStr, Expression<Func<T, bool>> whereLambda)
        {
            return dbRead.Set<T>().Where(whereLambda).OrderBy(orderStr, isAsc).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        private static readonly object LockSave = new object();
        private const int TrySaveCount = 2;  //服务端重试2次

        /// <summary>
        /// 提交保存更改
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            lock (LockSave)
            {
                int iSave = 1;  //最后一次写入失败后抛出异常
                int result = 0;

                #region 重复写，最后一次抛出异常

                while (iSave < TrySaveCount)
                {
                    try
                    {
                        result = dbWrite.SaveChanges();   //保存提交
                        return result > 0; //
                    }
                    catch (Exception)  //写入失败
                    {
                        Console.WriteLine($"!!!!!!!!!!! 第【{iSave}】次写入数据库失败 !!!!!!!!!!!!");
                        iSave++;
                        Thread.Sleep(100); //暂停100ms
                    }
                }

                #endregion

                #region 最后一次写入，失败后抛出异常

                try
                {
                    result = dbWrite.SaveChanges(); //保存提交
                    return result > 0; //
                }
                catch (Exception ex) //写入失败
                {
                    Console.WriteLine($"!!!!!!!!!!! 多次写入数据库失败，共尝试【{TrySaveCount}】次，请检查 !!!!!!!!!!!!");
                    dbWrite = DBContextFactory.GetService(); //保存失败，重新获取一个EF缓存
                    throw new Exception(
                        $"写入数据库失败，\r\n错误为：Message:{ex.Message}\r\nStackTrace:{ex.StackTrace}\r\nInnerException:{ex.InnerException?.Message}");
                }

                #endregion
            }
        }

        public T GetFirstOrDefault(string keyvalue)
        {
            return dbRead.Set<T>().Find(keyvalue);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> whereLambda)
        {
            return dbRead.Set<T>().FirstOrDefault(whereLambda);
        }

        public void Dispose()
        {
            dbWrite.Dispose();
        }
    }
}
