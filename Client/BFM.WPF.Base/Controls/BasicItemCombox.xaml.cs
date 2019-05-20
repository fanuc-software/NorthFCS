using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;

namespace BFM.WPF.Base.Controls
{
    /// <summary>
    /// BasicItemCombox.xaml 的交互逻辑
    /// Text="{Binding ISENABLE}" EnumIdentify="基础信息.是否启用"
    /// </summary>
    public partial class BasicItemCombox : ComboBox
    {
        private WcfClient<ISDMService> ws;
        private List<SysEnumItems> ItemData = new List<SysEnumItems>();

        private bool _bFinishInitial = false;
        

        public BasicItemCombox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 控件加载时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeSelectItem(Text, Value);
        }

        private int enumType;

        /// <summary>
        /// 下拉框类型 
        /// 0：可写，不学习；1：可写，可学习2：只下拉，不可写；
        /// </summary>
        private int EnumType
        {
            get { return enumType; }
            set
            {
                enumType = value;
                if ((enumType == 0) || (enumType == 1))
                {
                    this.IsEditable = true;
                }
                else
                {
                    this.IsEditable = false;
                }
            }
        }

        /// <summary>
        /// 值保存的字段名类型
        /// 0：ItemName 名称（默认）；1：ItemNO 编号；2：ItemCode 代码；3：PKNO 唯一标志（Sys_EnumItems.PKNO）
        /// </summary>
        private int ValueField { get; set; }


        /// <summary>
        /// Value值
        /// </summary>
        public string Value
        {
            get
            {
                if ((string.IsNullOrEmpty(SelectedValuePath)) || (SelectedValue == null))
                {
                    return Text;
                }
                else
                {
                    return SelectedValue.ToString();
                }
            }
        }

        #region Properties

        #region Properties.Webservice Url 

        public static readonly DependencyProperty WSDLUrlProperty =
            DependencyProperty.Register("WSDLUrl",
                typeof (string),
                typeof (BasicItemCombox),
                new FrameworkPropertyMetadata("", WSDLUrlChanged));

        private static void WSDLUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public string WSDLUrl
        {
            get { return (string) GetValue(WSDLUrlProperty); }
            set { SetValue(WSDLUrlProperty, value); }
        }

        #endregion

        #region Properties.Webservice GetItemDataMethod

        public static readonly DependencyProperty GetItemDataMethodProperty =
            DependencyProperty.Register("GetItemDataMethod",
                typeof (string),
                typeof (BasicItemCombox),
                new FrameworkPropertyMetadata("", GetItemDataMethodChanged));

        private static void GetItemDataMethodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public string GetItemDataMethod
        {
            get { return (string) GetValue(GetItemDataMethodProperty); }
            set { SetValue(GetItemDataMethodProperty, value); }
        }

        #endregion

        #region Properties.基础信息标识

        public static readonly DependencyProperty EnumIdentifyProperty =
            DependencyProperty.Register("EnumIdentify",
                typeof (string),
                typeof (BasicItemCombox),
                new FrameworkPropertyMetadata("", EnumIdentifyChanged));

        private static void EnumIdentifyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BasicItemCombox combox = d as BasicItemCombox;
            combox?.Initial();
        }

        public string EnumIdentify
        {
            get { return (string) GetValue(EnumIdentifyProperty); }
            set { SetValue(EnumIdentifyProperty, value); }
        }

        #endregion

        #region Properties.Text的格式，显示的Text的格式

        public static readonly DependencyProperty EnumTextForamtProperty =
            DependencyProperty.Register("EnumTextForamt",
                typeof (string),
                typeof (BasicItemCombox),
                new FrameworkPropertyMetadata("{ItemName}({ItemNO})", EnumTextForamtChanged));

        private static void EnumTextForamtChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public string EnumTextForamt
        {
            get { return (string) GetValue(EnumTextForamtProperty); }
            set { SetValue(EnumTextForamtProperty, value); }
        }

        #endregion
        
        #endregion

        /// <summary>
        /// 控件初始化，当EnumIdentify不为空时初始化，只初始化一次
        /// 提取数据库中控件的类型，数据
        /// </summary>
        private void Initial()
        {
            if ((string.IsNullOrEmpty(EnumIdentify)) || (_bFinishInitial))  //已完成初始化
            {
                return;
            }

            try
            {
                if (ws == null)
                {
                    ws = new WcfClient<ISDMService>();
                }

                SysEnumMain main = ws.UseService(s => s.GetSysEnumMains($"ENUM_IDENTIFY = '{EnumIdentify}'")).FirstOrDefault();
                if (main != null)
                {
                    #region 获取基础信息

                    EnumType = main.ENUM_TYPE;  //设置下拉框属性
                    ValueField = main.VALUE_FIELD; //0：ItemName 名称（默认）；1：ItemNO 编号；2：ItemCode 代号；3：PKNO 唯一标志（Sys_EnumItems.PKNO）

                    ItemData = ws.UseService(s => s.GetSysEnumItemss($"ENUM_IDENTIFY = '{main.ENUM_IDENTIFY}'"))
                            .OrderBy(c => c.ITEM_INDEX)
                            .ToList();
                    this.ItemsSource = ItemData;
                    this.DisplayMemberPath = "ITEM_NAME";

                    #region 设置Value的绑定路径

                    switch (ValueField)
                    {
                        case 1:  //编号
                            SelectedValuePath = "ITEM_NO";
                            break;
                        case 2:  //代码
                            SelectedValuePath = "ITEM_CODE";
                            break;
                        case 3:  //PKNO
                            SelectedValuePath = "PKNO";
                            break;
                        default:  //名称
                            SelectedValuePath = "ITEM_NAME";
                            break;
                    }

                    #endregion


                    #endregion
                }
                else
                {
                    #region 新增主信息

                    string[] mainTexts = EnumIdentify.Split('.');
                    string sort = mainTexts[0];
                    string name = mainTexts.Length < 2 ? mainTexts[0] : mainTexts[1];
                    for (int i = 2; i < mainTexts.Length; i++)
                    {
                        name += "." + mainTexts[i];
                    }

                    main = new SysEnumMain()
                    {
                        PKNO = CBaseData.NewGuid(),
                        COMPANY_CODE = "",
                        ENUM_IDENTIFY = this.EnumIdentify,
                        ENUM_SORT = sort,
                        ENUM_NAME = name,
                        ENUM_INTROD = "",
                        ENUM_TYPE = 0,
                        //ValueField = 0,
                        CREATION_DATE = DateTime.Now,
                        CREATED_BY = CBaseData.LoginName,
                        LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                        USE_FLAG = 1,
                        REMARK = "系统自动添加",
                    };
                    ws.UseService(s => s.AddSysEnumMain(main));

                    #endregion
                }
                _bFinishInitial = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error:BasicItemCombox.Initial " + ex.Message);
            }
        }

        private void ChangeSelectItem(string showText, string showValue)
        {
            if (!_bFinishInitial)  //还没有初始化，则初始化控件
            {
                Initial();
            }
            string selectValue = showValue;

            #region 获取Combobox初始的选择值

            if (string.IsNullOrEmpty(selectValue))
            {
                var item = ItemData.FirstOrDefault(c => c.ITEM_NAME == showText);  //按照Text查找
                if (item == null)
                {
                    selectValue = "";
                }
                else
                {
                    switch (ValueField)
                    {
                        case 1:  //编号
                            selectValue = item.ITEM_NO;
                            break;
                        case 2:  //代码
                            selectValue = item.ITEM_CODE;
                            break;
                        case 3:  //PKNO
                            selectValue = item.PKNO;
                            break;
                        default:  //名称
                            selectValue = item.ITEM_NAME;
                            break;
                    }
                }
            }

            #endregion

            this.SelectedValue = selectValue;
        }
        
        /// <summary>
        /// 保存 自学习情况
        /// </summary>
        public void Save()
        {
            if (string.IsNullOrEmpty(this.Text) || string.IsNullOrEmpty(this.Value) || (EnumType != 1)) //不自学习
            {
                return;
            }
            ComboBoxItem select = SelectedItem as ComboBoxItem;
            if (select != null) //是选择的下拉框，则不需要自学习
            {
                return;
            }
            try
            {
                if (ws == null)
                {
                    ws = new WcfClient<ISDMService>();
                }

                #region  按照Value自学习

                string sValue = "";//this.Value;
                string sName = this.Text;
                string sNO = this.Text;
                SysEnumItems item = new SysEnumItems()
                {
                    PKNO = CBaseData.NewGuid(),
                    COMPANY_CODE = "",
                    CREATED_BY = CBaseData.LoginName,
                    CREATION_DATE = DateTime.Now,
                    LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                    ENUM_IDENTIFY = this.EnumIdentify,
                    USE_FLAG = 1,
                    ITEM_CODE = "",
                    ITEM_NAME = sName,
                    ITEM_NO = sNO,
                    ITEM_INDEX = this.Items.Count,
                };
                ws.UseService(s => s.AddSysEnumItems(item));

                sValue = item.ITEM_NAME;

                ComboBoxItem comboBoxItem = new ComboBoxItem()
                {
                    Content = this.Text,
                    Tag = sValue,
                };
                this.Items.Add(comboBoxItem);
                this.SelectedIndex = this.Items.Count - 1;
                this.SelectedItem = comboBoxItem;

                #endregion
            }
            catch (Exception ex)
            {
                // ignored
                Console.WriteLine("error:BasicItemCombox.Save " + ex.Message);
            }
        }
        
    }
}
