using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFM.Server.DataAsset
{

    public interface IObjectFacade<T>
    {
        Task<List<T>> GetPageData(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere, string controllerName = null);
        Task<int> GetCount(string sWhere, string controllerName = null);
        Task<bool> Add(T t, string controllerName = null);
        Task<bool> Update(T t, string controllerName = null);
        Task<bool> Deletes(string[] keyvalues, string controllerName = null);
        Task<bool> Delete(string keyvalue, string controllerName = null);
        Task<List<T>> GetByParam(string sWhere, string controllerName = null);
        Task<T> GetById(string keyvalue, string controllerName = null);
    }
}
