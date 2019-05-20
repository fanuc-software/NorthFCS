using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;
using BFM.Common.Base;
using BFM.Common.Base.Utils;
using BFM.Common.DataBaseAsset.DataFilters;
using BFM.Common.DataBaseAsset.EF;

namespace BFM.Web.Base.WebApi
{
    public abstract class ObjectController<T> : ApiController where T : class, new()
    {
        public IBaseBLL<T> Bll { get; set; }
        public abstract void SetBll();

        public ObjectController()
        {
            SetBll();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Add([FromBody]string obj)
        {
            try
            {
                T t = SafeConverter.JsonDeserializeObject<T>(obj);

                return Bll.Add(t);
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public bool AddEntity([FromBody]List<string> entitys)
        {
            try
            {
                List<T> tsList = entitys.Select(SafeConverter.JsonDeserializeObject<T>).ToList();

                return Bll.AddEntity(tsList);
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public bool Update([FromBody]string obj)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public bool Delete([FromBody]string obj)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public bool Delete(List<T> entitys)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public bool Delete(Expression<Func<T, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public int GetCount(Expression<Func<T, bool>> WhereLambda)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public List<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public List<T> GetModelsByPage(int pageSize, int pageIndex, bool isAsc, string orderStr, Expression<Func<T, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public T GetFirstOrDefault(string keyvalue)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public T GetFirstOrDefault(Expression<Func<T, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }
    }
}
