using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BFM.WPF.WMS.ViewModel
{
    public class InventoryGraphicViewModel: INotifyPropertyChanged
    {
        #region 属性

        //总列数
        private int totalColumn = 6;
        public int TotalColumn
        {
            get { return totalColumn; }
            set
            {
                totalColumn = value;
                OnPropertyChanged("TotalColumn");
            }
        }

        //总层数
        private int totalLayer = 2;
        public int TotalLayer
        {
            get { return totalLayer; }
            set
            {
                totalLayer = value;
                OnPropertyChanged("TotalLayer");
            }
        }

        ///动作类型 0无；1入库中；2：原料加工；3：出库中；4：转换中；5：添加库存；6：只上料，首件检测；7：只下料/只从三坐标上料架
        /// 11：可入库；12：可加工；13：可出库/可转换；14：有库存
        private int actionType = 0;
        /// <summary>
        /// 动作类型 0无；1入库中；2：原料加工；3：出库中；4：转换中；5：添加库存；6：只上料，首件检测；7：只下料/只从三坐标上料架
        /// 11：可入库；12：可加工；13：可出库/可转换；14：有库存
        /// </summary>
        public int ActionType
        {
            get { return actionType; }
            set
            {
                actionType = value;
                OnPropertyChanged("ActionType");
            }
        }

        //货位PKNO
        private string allcationPKON = "";
        public string ALLOCATION_CODE
        {
            get { return allcationPKON; }
            set
            {
                allcationPKON = value;
                OnPropertyChanged("ALLOCATION_CODE");
            }
        }

        //货位名称
        private string curAlloName = "";
        public string CurAlloName
        {
            get { return curAlloName; }
            set
            {
                curAlloName = value;
                OnPropertyChanged("CurAlloName");
            }
        }

        //列
        private int curAlloCol = 1;
        public int CurAlloCol
        {
            get { return curAlloCol; }
            set
            {
                curAlloCol = value;
                OnPropertyChanged("CurAlloCol");
            }
        }

        //层
        private int curAlloLay = 1;
        public int CurAlloLay
        {
            get { return curAlloLay; }
            set
            {
                curAlloLay = value;
                OnPropertyChanged("CurAlloLay");
            }
        }

        //库存物料信息
        public string CurAlloItemPKNO = "";

        //库存物料
        private string curAlloItemName = "";
        public string CurAlloItemName
        {
            get { return curAlloItemName; }
            set
            {
                curAlloItemName = value;
                OnPropertyChanged("CurAlloItemName");
            }
        }

        //库存型号
        private string curAlloItemNorm = "";
        public string CurAlloItemNorm
        {
            get { return curAlloItemNorm; }
            set
            {
                curAlloItemNorm = value;
                OnPropertyChanged("CurAlloItemNorm");
            }
        }

        //属性
        private string curAlloItemType = "";
        public string CurAlloItemType
        {
            get { return curAlloItemType; }
            set
            {
                curAlloItemType = value;
                OnPropertyChanged("CurAlloItemType");
            }
        }

        //数量
        private string curAlloNumber = "";
        public string CurAlloNumber
        {
            get { return curAlloNumber; }
            set
            {
                curAlloNumber = value;
                OnPropertyChanged("CurAlloNumber");
            }
        }

        //备注
        private string curInvRemark = "";
        public string CurInvRemark
        {
            get { return curInvRemark; }
            set
            {
                curInvRemark = value;
                OnPropertyChanged("CurInvRemark");
            }
        }
        

        #endregion

        #region mvvm

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
