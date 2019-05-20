using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace BFM.Common.Base.Helper
{
    /// <summary>
    /// 动态执行代码
    /// </summary>
    public static class DynamicCode
    {
        /// <summary>
        /// 执行带返回值的代码
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultType"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static T ExecReturnCode<T>(string resultType, string code)
        {
            string classname = "C" + Guid.NewGuid().ToString("N");
            string funcName = "ExecReturnCode";
            string buildCode = BuildExecCode(classname, funcName, resultType, code);  //形成执行的代码

            return (T)ExecBehideCode(classname, funcName, buildCode);  //执行代码 并获取返回值
        }

        /// <summary>
        /// 执行代码组的返回结果，增加效率
        /// 实例
        /// List<object> values = new List<object>();
        /// string[,] resultTypeAndCodes = new string[3,2]
        /// {
        /// {"double", tbSource.Text },
        /// {"double", tbSource2.Text },
        /// {"double", tbSource3.Text },
        /// };
        /// DynamicCode.ExecReturnCodeByList(resultTypeAndCodes, ref values);
        /// tbResult.Text = values[0].ToString();
        /// tbResult2.Text = values[1].ToString();
        /// tbResult3.Text = values[2].ToString();
        /// </summary>
        public static void ExecReturnCodeByList(string[,] resultTypeAndCodes, ref List<object> results)
        {
            if (resultTypeAndCodes == null) throw new ArgumentNullException(nameof(resultTypeAndCodes));
            string className = "C" + Guid.NewGuid().ToString("N");

            string execCode = "using System; \r\n" +
                              "using System.Text; \r\n" +
                              "using System.Collections.Generic; \r\n";

            execCode += "public class " + className + "\r\n" +
                        "{ \r\n";

            string baseFunc = "ExecReturnCodeByList";
            for (int i = 0; i < resultTypeAndCodes.GetLength(0); i++)
            {
                string resultType = resultTypeAndCodes[i, 0];
                string code = resultTypeAndCodes[i, 1].Trim();

                string funcName = baseFunc + i;

                execCode += BuildExecFunc(funcName, resultType, code) + "\r\n";
            }

            execCode += "} \r\n";

            try
            {
                CodeDomProvider compiler = new CSharpCodeProvider();
                CompilerParameters cp= new CompilerParameters() { GenerateExecutable = false, GenerateInMemory = true, };
                CompilerResults cr = compiler.CompileAssemblyFromSource(cp, execCode);
                if (cr.Errors.HasErrors)
                {
                    throw new Exception("Invaild Code: " + execCode);
                }

                Assembly a = cr.CompiledAssembly;
                Type t = a.GetType(className);
                results = new List<object>();

                for (int i = 0; i < resultTypeAndCodes.GetLength(0); i++)
                {
                    object obj = t.InvokeMember(baseFunc + i,
                        BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
                        System.Type.DefaultBinder, null, null);

                    results.Add(obj);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Invaild Code: " + execCode + " error:" + e.Message);
            }
        }

        /// <summary>
        /// 执行代码，无返回值
        /// </summary>
        /// <param name="code"></param>
        public static void ExecCode(string code)
        {
            string classname = "C" + Guid.NewGuid().ToString("N");
            string funcName = "ExecCode";
            string buildCode = BuildExecCode(classname, funcName, "", code);  //形成执行的代码

            ExecBehideCode(classname, funcName, buildCode);  //执行代码
        }

        /// <summary>
        /// 形成执行的函数
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="resultType"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string BuildExecFunc(string funcName, string resultType, string code, List<string> funcParams = null)
        {
            code = code.Trim().TrimEnd(';');

            if ((string.IsNullOrEmpty(resultType)) || (resultType.ToLower() == "void")) //无返回值
            {
                resultType = "void";
            }
            else //有返回值
            {
                if (!code.Contains("return ")) //没有return 时
                {
                    code = "return (" + code + ")" + ((resultType == "string") ? ".ToString();" : ";");
                }
            }

            code = code.TrimEnd(';') + ";";

            string param = (funcParams != null) ? string.Join(", ", funcParams.ToArray()) : "";  //形成参数

            return "    public static " + resultType + " " + funcName + "(" + param + ") \r\n" +
                   "    { \r\n" +
                   "    " + code + "\r\n" +
                   "    } \r\n";
        }

        /// <summary>
        /// 形成执行的代码
        /// </summary>
        /// <param name="className"></param>
        /// <param name="funcName"></param>
        /// <param name="resultType"></param>
        /// <param name="code"></param>
        /// <param name="usings"></param>
        /// <returns></returns>
        private static string BuildExecCode(string className, string funcName, string resultType, string code, List<string> usings = null)
        {
            string execCode = "using System; \r\n" +
                   "using System.Text; \r\n" +
                   "using System.Collections.Generic; \r\n";
            
            if (usings != null)
            {
                foreach (var u in usings)
                {
                    string str = u.Trim().TrimEnd(';') + " ;";
                    if (!str.Contains("using "))
                    {
                        str = "using " + str;
                    }

                    execCode += str + "\r\n";
                }
            }
            
            execCode += "public class " + className + "\r\n" +
                   "{ \r\n" + BuildExecFunc(funcName, resultType, code) + "} \r\n";
            return execCode;
        }

        /// <summary>
        /// 执行代码
        /// </summary>
        /// <param name="className"></param>
        /// <param name="funcName"></param>
        /// <param name="buildCode"></param>
        /// <param name="referens"></param>
        /// <returns></returns>
        private static object ExecBehideCode(string className, string funcName, string buildCode, List<string> referens = null)
        {
            try
            {
                CodeDomProvider compiler = new CSharpCodeProvider();
                CompilerParameters cp;
                if (referens != null)
                {
                    cp = new CompilerParameters(referens.ToArray()) { GenerateExecutable = false, GenerateInMemory = true, };
                }
                else
                {
                    cp = new CompilerParameters() { GenerateExecutable = false, GenerateInMemory = true, };
                }
                CompilerResults cr = compiler.CompileAssemblyFromSource(cp, buildCode);
                if (cr.Errors.HasErrors)
                {
                    throw new Exception("Invaild Code: " + buildCode);
                }

                Assembly a = cr.CompiledAssembly;
                Type t = a.GetType(className);
                return t.InvokeMember(funcName,
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
                    System.Type.DefaultBinder, null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Invaild Code: " + buildCode + " error:" + e.Message);
            }
        }
    }
}
