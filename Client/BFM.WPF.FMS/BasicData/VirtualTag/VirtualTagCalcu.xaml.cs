using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BFM.Common.Base;
using BFM.Common.Base.Helper;
using BFM.Common.Base.Utils;
using BFM.Common.DeviceAsset;
using BFM.ContractModel;
using BFM.Server.DataAsset.FMSService;
using BFM.WPF.Base;
using BFM.WPF.Base.Helper;
using Microsoft.CSharp;

namespace BFM.WPF.FMS.BasicData.VirtualTag
{
    /// <summary>
    /// VirtualTagCalcu.xaml 的交互逻辑
    /// </summary>
    public partial class VirtualTagCalcu : UserControl, INotifyPropertyChanged
    {
        public List<FmsAssetTagSetting> DeviceTags = new List<FmsAssetTagSetting>();

        public VirtualTagCalcu()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ConditionCals.Count == 0)
            {
                ConditionCals.Add(new ConditionCal());
            }
        }

        #region 属性

        private int _calculationType = 1;
        /// <summary>
        /// 计算类型 1：逻辑运算；2：数值运算；3：字符运算；12：条件数值运算；13：条件字符运算；100：C#脚本
        /// </summary>
        public int CalculationType
        {
            get { return _calculationType; }
            set
            {
                _calculationType = value;
                OnPropertyChanged("CalculationType");
            }
        }

        private int _valueType = 0;
        /// <summary>
        /// 数据类型 0：默认
        /// </summary>
        public int ValueType
        {
            get { return _valueType; }
            set
            {
                _valueType = value;
                OnPropertyChanged("ValueType");
            }
        }

        private List<ComboxHelper> _allShowTags = new List<ComboxHelper>();

        public List<ComboxHelper> AllShowTags
        {
            get { return _allShowTags; }
            set
            {
                _allShowTags = value;
                OnPropertyChanged("AllTags");
            }
        }

        private string _calculationText;
        /// <summary>
        /// 计算表达式
        /// </summary>
        public string CalculationText
        {
            get { return _calculationText;}
            set
            {
                _calculationText = value;
                OnPropertyChanged("CalculationText");
            }
        }
        
        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                _isReadOnly = value;
                OnPropertyChanged("IsReadOnly");
                Dispatcher.Invoke(delegate { colOper.Visible = !_isReadOnly; });
            }
        }

        #region 逻辑运算

        private string _logicText;

        /// <summary>
        /// 逻辑运算
        /// </summary>
        public string LogicText
        {
            get { return _logicText; }
            set
            {
                _logicText = value;
                OnPropertyChanged("LogicText");
            }
        }

        #endregion

        #region 数值/字符运算

        private string _normalText;

        /// <summary>
        /// 数值/字符运算
        /// </summary>
        public string NormalText
        {
            get { return _normalText; }
            set
            {
                _normalText = value;
                OnPropertyChanged("NormalText");
            }
        }

        #endregion

        #region 条件计算相关

        private List<ConditionCal> _conditionCals = new List<ConditionCal>();
        /// <summary>
        /// 条件计算展示结果
        /// </summary>
        public List<ConditionCal> ConditionCals
        {
            get { return _conditionCals; }
            set
            {
                _conditionCals = value;
                OnPropertyChanged("ConditionCals");

                RefreshCalculationText();
            }
        }

        #endregion

        #region 标签数组序号

        private string _ArrayTagPKNO;

        /// <summary>
        /// 标签数组序号 Tag.PKNO
        /// </summary>
        public string ArrayTagPKNO
        {
            get { return _ArrayTagPKNO; }
            set
            {
                _ArrayTagPKNO = value;
                OnPropertyChanged("ArrayTagPKNO");
            }
        }


        private string _ArrayIndex;

        /// <summary>
        /// 标签数组序号 Index
        /// </summary>
        public string ArrayIndex
        {
            get { return _ArrayIndex; }
            set
            {
                _ArrayIndex = value;
                OnPropertyChanged("ArrayIndex");
            }
        }

        #endregion

        #region C#相关

        private string _cSharpText;

        /// <summary>
        /// C#表达式
        /// </summary>
        public string CSharpText
        {
            get { return _cSharpText; }
            set
            {
                _cSharpText = value;
                OnPropertyChanged("CSharpText");
            }
        }

        #endregion
        
        #endregion

        private TextBox CurEditText = null;

        #region 通用事件

        //增加点
        private void BtnAddTag_Click(object sender, RoutedEventArgs e)
        {
            //添加Tag点
            if (string.IsNullOrEmpty(CmbTags.Text))
            {
                return;
            }

            if (CurEditText == null)
            {
                return;
            }

            CurEditText.SelectedText = " {" + CmbTags.Text + "} ";

            CurEditText.Focus();
        }

        //测试
        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            TbTestResult.Text = "";
            RefreshCalculationText();

            Type dynamicCode = null; //获取编译后代码，调用该类用
            Dictionary<string, Dictionary<string, string>>
                FuncAndParamTagPKNO = new Dictionary<string, Dictionary<string, string>>(); //函数和对应参数的Tag的PKNO

            string className = "C" + Guid.NewGuid().ToString("N");

            try
            {
                Cursor = Cursors.Wait;

                #region 形成执行的代码

                string execCode = "using System; \r\n" +
                                  "using System.Text; \r\n" +
                                  "using System.Collections.Generic; \r\n" +
                                  "using BFM.Common.Base; \r\n\r\n";

                execCode += "public class " + className + "\r\n" +
                            "{ \r\n";

                string basicFuc = "AutoCalculation";

                int index = 1;
                FuncAndParamTagPKNO.Clear();

                string exp = CalculationText; //表达式

                string funcname = basicFuc + index.ToString(); //函数名称
                Dictionary<string, string> paramTas = new Dictionary<string, string>(); //参数对应的标签的PKNO, param名称

                List<string> funcParam = new List<string>(); //带类型的参数

                string code = "";

                foreach (var line in exp.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                {
                    string ret = line;

                    #region 替换标签值，将标签替换成参数名

                    string[] expTags = line.Split('{');

                    for (int i = 0; i < expTags.Length; i++)
                    {
                        string str = expTags[i];
                        int length = str.IndexOf('}');

                        if (length < 0) //没有找到  }
                        {
                            continue;
                        }

                        string tagPKNO = str.Substring(0, length); //{ } 内为PKNO

                        string param = "{" + tagPKNO + "}";

                        if (paramTas.ContainsKey(tagPKNO))  //已经添加了该参数
                        {
                            param = paramTas[tagPKNO];
                        }
                        else
                        {
                            FmsAssetTagSetting tag = DeviceTags.FirstOrDefault(s => s.PKNO == tagPKNO);
                            if (tag == null) continue;

                            param = "param" + paramTas.Count;
                            paramTas.Add(tagPKNO, param);
                            string paramType = "string";
                            //string paramType = ((CalculationType == 2) || (tag.VALUE_TYPE > 0 && tag.VALUE_TYPE < 20))
                            //    ? "double"
                            //    : "string";
                            funcParam.Add(paramType + " " + param);
                        }

                        ret = ret.Replace("{" + tagPKNO + "}", param);
                    }

                    #endregion

                    if (string.IsNullOrEmpty(code))
                    {
                        code = "    " + ret;
                    }
                    else
                    {
                        code += Environment.NewLine + "    " + ret;
                    }
                }

                //C#脚本
                //支持C#语法，最后返回值（Double/String)
                string resultType = "string";

                //确定返回结果类型，将code语句转换成C#的语句
                if (CalculationType == 1) //逻辑运算
                {
                    //（结果为1,0):({标签1}==1)&&({标签2}==1)&&({标签3}==0||{标签4}==0)&&({标签5}==1)
                    code = code.Replace("AND", "&&").Replace("and", "&&").Replace("OR", "||").Replace("or", "||");
                    resultType = "bool";
                }
                else if (CalculationType == 2) //数值运算
                {
                    //{标签1}+3+{标签2}+4
                    resultType = "double";
                }
                else if (CalculationType == 3) //字符运算
                {
                    //{标签1}+"123"
                }
                else if (CalculationType == 12) //条件数值运算
                {
                    //{标签1}==3:{标签2}+1;{标签1}==4:{标签2}+2;{标签1}==5:{标签2}+3
                    resultType = "double";
                    List<string> exps = code.Split(';').ToList();
                    string temp = "";
                    foreach (var exp1 in exps)
                    {
                        if (exp1.Split(':').Length < 2)
                        {
                            continue;
                        }
                        temp += "        if (" + exp1.Split(':')[0] + ") { return (" + exp1.Split(':')[1] + "); } \r\n";
                    }

                    temp += "        return 0; \r\n";

                    code = temp;
                }
                else if (CalculationType == 13) //条件字符运算
                {
                    //{标签1}==3:{标签1}+"123";{标签1}==4:{标签1}+"123"
                    List<string> exps = code.Split(';').ToList();
                    string temp = "";
                    foreach (var exp1 in exps)
                    {
                        if (exp1.Split(':').Length < 2)
                        {
                            continue;
                        }
                        temp += "        if (" + exp1.Split(':')[0] + ") { return (" + exp1.Split(':')[1] + ").ToString(); } \r\n";
                    }

                    temp += "        return \"\"; \r\n";

                    code = temp;
                }
                else if (CalculationType == 21)
                {
                    resultType = "string";//{标签1};3
                    List<string> exps = code.Split(';').ToList();
                    string temp = "";
                    if (exps.Count >= 2)
                    {
                        int arrayIndex = SafeConverter.SafeToInt(exps[1].Trim(), 0);
                        temp += "            if ( " + exps[0].Trim() + ".Split('|').Length > " + arrayIndex + ") { return " + exps[0].Trim() + ".Split('|')[" + arrayIndex + "]; } \r\n";
                    }

                    temp += "        return \"\"; \r\n";

                    code = temp;
                }
                else if (CalculationType == 100) //C#脚本
                {
                    //支持C#语法，最后返回值（Double/String)
                    resultType = "string";
                }
                else  //不支持的类型
                {
                    code = $"        return \"计算类型[{CalculationType}]，不支持的类型。\"; \r\n";
                }

                execCode += DynamicCode.BuildExecFunc(funcname, resultType, code, funcParam);

                FuncAndParamTagPKNO.Add(funcname, paramTas); //添加

                execCode += "}\r\n";

                #endregion

                #region 编译代码

                CodeDomProvider compiler = new CSharpCodeProvider();
                CompilerParameters cp = new CompilerParameters() { GenerateExecutable = false, GenerateInMemory = true, };
                cp.ReferencedAssemblies.Add("BFM.Common.Base.dll");
                CompilerResults cr = compiler.CompileAssemblyFromSource(cp, execCode);
                if (cr.Errors.HasErrors)
                {
                    WPFMessageBox.ShowError("测试失败，语法错误.\r\n" + execCode, "测试");
                    return;
                }

                dynamicCode = cr.CompiledAssembly.GetType(className); //获取

                #endregion

                #region 获取值

                index = 0;
                string funcName = FuncAndParamTagPKNO.Keys.ToList()[index];
                var tagParms = FuncAndParamTagPKNO.Values.ToList()[index];
                List<object> paramValues = new List<object>(); //参数值

                foreach (var tagpkno in tagParms) //参数 
                {
                    object value;
                    FmsAssetTagSetting tagParam = DeviceTags.FirstOrDefault(s => s.PKNO == tagpkno.Key);
                    
                    if (tagParam != null)
                    {
                        value = SafeConverter.SafeToStr(tagParam.CUR_VALUE);
                        //if ((CalculationType == 2) || (tagParam.VALUE_TYPE > 0 && tagParam.VALUE_TYPE < 20))
                        //{
                        //    value = SafeConverter.SafeToDouble(tagParam.CUR_VALUE);
                        //}
                        //else
                        //{
                        //    value = SafeConverter.SafeToStr(tagParam.CUR_VALUE);
                        //}
                    }
                    else
                    {
                        value = "";
                    }

                    paramValues.Add(value);
                }

                object obj = dynamicCode.InvokeMember(funcName,
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
                    System.Type.DefaultBinder, null, paramValues.ToArray());

                string newValue = ""; //新的计算结果

                if (CalculationType == 1)  //逻辑运算
                {
                    newValue = SafeConverter.SafeToBool(obj) ? "1" : "0";
                }
                else
                {
                    newValue = SafeConverter.SafeToStr(obj);
                }

                Console.WriteLine("测试结果：" + newValue);

                TbTestResult.Text = newValue;

                #endregion

                WPFMessageBox.ShowInfo("测试成功. \r\n测试结果为：" + newValue, "测试");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                WPFMessageBox.ShowError("测试失败，错误为：" + ex.Message, "测试");
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        //鼠标双击 选择整个点
        private bool TextBoxDoubleClick(TextBox textBox)
        {
            int midIndex = textBox.SelectionStart;
            int curLineIndex = textBox.GetLineIndexFromCharacterIndex(midIndex);
            int startIndex1 = textBox.Text.Substring(0, midIndex).LastIndexOf('{');
            int startIndex2 = textBox.Text.Substring(0, midIndex).LastIndexOf('}');
            int endIndex1 = textBox.Text.IndexOf('{', midIndex);
            int endIndex2 = textBox.Text.IndexOf('}', midIndex);

            if ((startIndex1 <= startIndex2) || (endIndex1 >= 0 && endIndex1 <= endIndex2) ||
                (endIndex2 <= startIndex1))
            {
                return false;
            }

            if ((textBox.GetLineIndexFromCharacterIndex(startIndex1) != curLineIndex) ||
                (textBox.GetLineIndexFromCharacterIndex(endIndex2) != curLineIndex))
            {
                return false;
            }

            textBox.Select(startIndex1, endIndex2 - startIndex1 + 1);

            return true;
        }

        //控制键盘删除、移动选择整个点
        private bool TextBoxKeyDown(TextBox textBox, Key key)
        {
            if (!string.IsNullOrEmpty(textBox.SelectedText)) //有选中的内容
            {
                return false;
            }

            int curLineIndex = textBox.GetLineIndexFromCharacterIndex(textBox.SelectionStart);

            if ((key == Key.Delete) || (key == Key.Right)) //向右删除
            {
                int startIndex = textBox.SelectionStart;
                //获取下一个字符为 { 时
                if ((textBox.Text.Length <= startIndex) ||
                    (textBox.Text[startIndex] != '{'))
                {
                    return false;
                }

                int endIndex = textBox.Text.IndexOf('}', startIndex);
                if ((textBox.GetLineIndexFromCharacterIndex(endIndex) != curLineIndex) ||
                    (endIndex < startIndex))
                {
                    return false;
                }

                textBox.Select(startIndex, endIndex - startIndex + 1);

                return true;
            }
            else if ((key == Key.Back) || (key == Key.Left)) //向左删除
            {
                int endIndex = textBox.SelectionStart - 1;
                if ((endIndex <= 0) ||
                    (textBox.Text[endIndex] != '}'))
                {
                    return false;
                }

                int startIndex = textBox.Text.Substring(0, endIndex).LastIndexOf('{');

                if ((textBox.GetLineIndexFromCharacterIndex(startIndex) != curLineIndex) ||
                    (endIndex < startIndex))
                {
                    return false;
                }

                textBox.Select(startIndex, endIndex - startIndex + 1);

                return true;
            }

            return false;
        }

        //刷新计算表达式
        public string RefreshCalculationText()
        {
            //计算类型 1：逻辑运算；2：数值运算；3：字符运算；12：条件数值运算；13：条件字符运算；100：C#脚本
            if (CalculationType == 1) //逻辑运算
            {
                string calculationText = LogicText;

                foreach (var item in AllShowTags)
                {
                    calculationText = calculationText.Replace("{" + item.Name + "}", "{" + item.Value + "}");
                }

                CalculationText = calculationText;
            }
            else if (CalculationType == 2) //数值运算
            {
                string calculationText = NormalText;

                foreach (var item in AllShowTags)
                {
                    calculationText = calculationText.Replace("{" + item.Name + "}", "{" + item.Value + "}");
                }

                CalculationText = calculationText;
            }
            else if (CalculationType == 3) //字符运算
            {
                string calculationText = NormalText;

                foreach (var item in AllShowTags)
                {
                    calculationText = calculationText.Replace("{" + item.Name + "}", "{" + item.Value + "}");
                }

                CalculationText = calculationText;
            }
            else if (CalculationType == 12) //条件数值运算
            {
                string calculationText = "";

                foreach (var cal in ConditionCals)
                {
                    if (string.IsNullOrEmpty(cal.Condition) || string.IsNullOrEmpty(cal.Result))
                    {
                        continue;
                    }

                    string str = cal.ToString();

                    foreach (var item in AllShowTags)
                    {
                        str = str.Replace("{" + item.Name + "}", "{" + item.Value + "}");
                    }

                    calculationText += (string.IsNullOrEmpty(calculationText) ? "" : ";") + str;
                }

                CalculationText = calculationText;
            }
            else if (CalculationType == 13) //条件字符运算
            {
                string calculationText = "";

                foreach (var cal in ConditionCals)
                {
                    if (string.IsNullOrEmpty(cal.Condition))
                    {
                        continue;
                    }

                    string str = cal.ToString();

                    foreach (var item in AllShowTags)
                    {
                        str = str.Replace("{" + item.Name + "}", "{" + item.Value + "}");
                    }

                    calculationText += (string.IsNullOrEmpty(calculationText) ? "" : ";") + str;
                }

                CalculationText = calculationText;
            }
            else if (CalculationType == 21) //标签数组序号
            {
                CalculationText = "{" + ArrayTagPKNO + "};" + ArrayIndex;
            }
            else if (CalculationType == 100) //C#脚本
            {
                string calculationText = CSharpText;

                foreach (var item in AllShowTags)
                {
                    calculationText = calculationText.Replace("{" + item.Name + "}", "{" + item.Value + "}");
                }

                CalculationText = calculationText;
            }

            return CalculationText;
        }

        #endregion

        #region 逻辑计算

        private void TbLogicText_GotFocus(object sender, RoutedEventArgs e)
        {
            CurEditText = TbLogicText;
        }

        private void TbLogicText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = TextBoxKeyDown(TbLogicText, e.Key);
        }

        private void TbLogicText_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = TextBoxDoubleClick(TbLogicText);
        }

        #endregion

        #region 数值/字符运算

        private void TbNormalCal_GotFocus(object sender, RoutedEventArgs e)
        {
            CurEditText = TbNormalCal;
        }

        private void TbNormalCal_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = TextBoxKeyDown(TbNormalCal, e.Key);
        }

        private void TbNormalCal_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = TextBoxDoubleClick(TbNormalCal);
        }

        #endregion

        #region 条件计算相关

        private void TbCondition_GotFocus(object sender, RoutedEventArgs e)
        {
            CurEditText = TbCondition;
        }

        private void TbResult_GotFocus(object sender, RoutedEventArgs e)
        {
            CurEditText = TbResult;
        }

        private void TbCondition_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = TextBoxKeyDown(TbCondition, e.Key);
        }

        private void TbCondition_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = TextBoxDoubleClick(TbCondition);
        }

        private void TbResult_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = TextBoxKeyDown(TbResult, e.Key);
        }

        private void TbResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = TextBoxDoubleClick(TbResult);
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            ConditionCal item0 = new ConditionCal();
            List<ConditionCal> cons = new List<ConditionCal>(ConditionCals.ToArray());
            cons.Add(item0);
            ConditionCals = cons;

            //gridTagItems.SelectedItem = ConditionCals[ConditionCals.Count - 1];
        }

        private void BtnDel_OnClick(object sender, RoutedEventArgs e)
        {
            List<ConditionCal> cons = new List<ConditionCal>(ConditionCals.ToArray());
            ConditionCal item0 = gridTagItems.SelectedItem as ConditionCal;

            if (item0 != null) cons.Remove(item0);
            if (cons.Count == 0)
            {
                cons.Add(new ConditionCal());
            }
            ConditionCals = cons;
        }

        #endregion

        #region C#脚本

        private void TbCSharpScript_GotFocus(object sender, RoutedEventArgs e)
        {
            CurEditText = TbCSharpScript;
        }

        private void TbCSharpScript_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = TextBoxKeyDown(TbCSharpScript, e.Key);
        }

        private void TbCSharpScript_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = TextBoxDoubleClick(TbCondition);
        }

        #endregion

        #region mvvm 的回调事件

        //Set 中 增加 OnPropertyChanged("Value");

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class ConditionCal: INotifyPropertyChanged
    {
        public string PKNO { get; set; }
        private string _condition;
        public string Condition
        {
            get { return _condition;}
            set
            {
                _condition = value;
                OnPropertyChanged("Condition");
            }
        }
        private string _result;
        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }

        }
        public ConditionCal()
        {
            PKNO = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return Condition.Replace(" = ", " == ") + ":" + Result;
        }

        #region mvvm 的回调事件

        //Set 中 增加 OnPropertyChanged("Value");

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }



}
