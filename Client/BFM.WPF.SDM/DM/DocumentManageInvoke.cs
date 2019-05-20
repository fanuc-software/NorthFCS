using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using BFM.Common.Base.Utils;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Xpf.Core;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;

namespace BFM.WPF.SDM.DM
{
   public class DocumentManageInvoke
    {

        private WcfClient<ISDMService> _wsClient;

       public DocumentManageInvoke()
       {
            _wsClient = new WcfClient<ISDMService>();
        }
        /// <summary>
        /// 新建文档管理弹窗
        /// </summary>
        /// <param name="beLongFunction">所属模块</param>
        /// <param name="functionPKNO">所属模块PKNO</param>
        /// <param name="Istate">文档状态 0：普通文档 1：封面</param>
        /// <param name="groupNo">分组编号</param>
        /// <param name="mode">是否可读可写</param>
        public static void NewDocumentManage(string beLongFunction, string functionPKNO,string groupNo,int Istate, DocumentMangMode mode)
        {
       
            DXWindow window = new DXWindow();
            DocumentManageView documentManageView = new DocumentManageView(mode);
              
            documentManageView.BelongFunction = beLongFunction;
            documentManageView.FunctionPKNO = functionPKNO;
            documentManageView.GroupNo = groupNo;
    
            //documentManageView.IsRead = mode;
            documentManageView.BindGridView();
            documentManageView.Istate = Istate;
            window.WindowStartupLocation=WindowStartupLocation.CenterScreen;
            window.Content = documentManageView;
            window.ShowDialog();
        }
    }

    /// <summary>
    /// 文档管理模式
    /// </summary>
    public enum DocumentMangMode
    {
        /// <summary>
        /// 只读
        /// </summary>
        ReadOnly = 1,
        /// <summary>
        /// 可上传
        /// </summary>
        CanUpLoad = 0,
    }
}
