using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.WPF.SPSS
{
    class ProductionConvert
    {

        public ProductionConvert(Int32? PART_NUM,Int32? TOTAL_PART_NUM)
        {
            _PART_NUM = PART_NUM;
            _TOTAL_PART_NUM = TOTAL_PART_NUM;
        }


        #region 工件计数

        private Int32? _PART_NUM;
        /// <summary> 
        ///  工件计数
        /// </summary> 
 
        public Int32? PART_NUM
        {
            get { return _PART_NUM; }
            set { _PART_NUM = value; }
        }

        #endregion 工件计数

        #region 工件总数

        private Int32? _TOTAL_PART_NUM;
        /// <summary> 
        ///  工件总数
        /// </summary> 
 
        public Int32? TOTAL_PART_NUM
        {
            get { return _TOTAL_PART_NUM; }
            set { _TOTAL_PART_NUM = value; }
        }

        #endregion 工件总数
    }
}
