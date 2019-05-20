using System.Threading;
using System.Windows;
using System.Windows.Media;
using BFM.Common.Base.PubData;

namespace BFM.WPF.FlowDesign.Common
{
    public static class ControlsCommon
    {
        #region 递归找父级控件

        /// <summary>
        /// 递归找父级控件
        /// </summary>
        /// <param name="reference">当前对象</param>
        /// <returns>父类控件</returns>
        public static T FindParentControl<T>(DependencyObject reference)
        {
            while (!CBaseData.AppClosing)
            {
                DependencyObject dObj = VisualTreeHelper.GetParent(reference);
                if (dObj == null) return default(T);

                if (dObj.GetType() == typeof (T))  //找到匹配的类型，并返回
                {
                    object o = dObj;
                    return (T) o;
                }

                reference = dObj;

                Thread.Sleep(10);
            }

            return default(T);
        }

        #endregion
    }
}
