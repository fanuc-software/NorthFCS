using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.FMSService;
using BFM.Server.DataAsset.SQLService;
using BFM.WPF.Base;

namespace BFM.WPF.FMS.ProcessControl
{
    /// <summary>
    /// ActionFomulaMain.xaml 的交互逻辑
    /// </summary>
    public partial class ActionFomulaMain : Window
    {
        private WcfClient<IFMSService> ws = new WcfClient<IFMSService>();

        private string formulaCode = "";

        private ActionFomulaMain()
        {
            InitializeComponent();
        }

        public ActionFomulaMain(FmsActionFormulaMain formulaMain)
        {
            InitializeComponent();

            if (formulaMain == null)
            {
                formulaMain = new FmsActionFormulaMain()
                {
                    PKNO = "",
                    USE_FLAG = 1,
                };
            }
            formulaCode = formulaMain.FORMULA_CODE;
            gbInfo.DataContext = formulaMain;

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            FmsActionFormulaMain main = gbInfo.DataContext as FmsActionFormulaMain;
            if (main == null)
            {
                return;
            }

            #region  检验

            if (string.IsNullOrEmpty(main.FORMULA_CODE))
            {
                WPFMessageBox.ShowError("请输入动作配方编码", "保存");
                return;
            }

            var check = ws.UseService(s =>
                s.GetFmsActionFormulaMains(
                    $"USE_FLAG = 1 AND FORMULA_CODE = '{main.FORMULA_CODE}' AND PKNO <> '{main.PKNO}'"));
            if (check.Any())
            {
                WPFMessageBox.ShowError($"输入动作配方编码已经存在不能{(string.IsNullOrEmpty(main.PKNO) ? "新增" : "修改")}该编码，请核实", "保存");
                return;
            }

            #endregion

            if (string.IsNullOrEmpty(main.PKNO))  //新增
            {

                main.PKNO = CBaseData.NewGuid();
                main.CREATION_DATE = DateTime.Now;
                main.CREATED_BY = CBaseData.LoginName;
                main.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                ws.UseService(s => s.AddFmsActionFormulaMain(main));
            }
            else
            {
                main.UPDATED_BY = CBaseData.LoginName;
                main.LAST_UPDATE_DATE = DateTime.Now;
                ws.UseService(s => s.UpdateFmsActionFormulaMain(main));
            }

            if (!string.IsNullOrEmpty(formulaCode))
            {
                string sql =
                    $"UPDATE FMS_ACTION_FORMULA_DETAIL SET FORMULA_CODE = '{main.FORMULA_CODE}' WHERE FORMULA_CODE = '{formulaCode}'";
                WcfClient<ISQLService> wsSQL = new WcfClient<ISQLService>();

                wsSQL.UseService(s => s.ExecuteSql(sql));
            }

            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
