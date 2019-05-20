using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using BFM.Common.Base.Utils;

namespace BFM.Common.Base.Helper
{
    /***********************************************
     *  
     *  枚举帮助类
     *
     ***********************************************/
    public static class EnumHelper
    {
        private class EnumList
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        /// <summary>
        /// 将枚举中的所有枚举类型转换成DataTable
        /// </summary>
        /// <typeparam name="T">枚举类型名称</typeparam>
        /// <returns>Name 枚举名称；Value 枚举值</returns>
        public static DataTable GetEnumsToList<T>() where T : struct
        {
            DataTable table = new DataTable("GetEnumsToList");
            table.Columns.Add("Name", typeof(String));
            table.Columns.Add("Value", typeof(Int32));

            try
            {
                Type t = typeof(T);

                if (!t.IsEnum)
                {
                    return table;
                }

                FieldInfo[] fieldinfo = t.GetFields(); //获取字段信息对象集合

                foreach (FieldInfo field in fieldinfo)
                {
                    if (!field.IsSpecialName)
                    {
                        DataRow row = table.NewRow();

                        object[] attrs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attrs.Length > 0)
                        {
                            row[0] = ((DescriptionAttribute)attrs[0]).Description;  //按照描述
                        }
                        else
                        {
                            row[0] = field.Name; //获取文本字段
                        }

                        row[1] = (int)field.GetRawConstantValue(); //获取int数值
                        table.Rows.Add(row);
                    }
                }
            }
            catch
            {
                // ignored
            }

            return table;
        }

        /// <summary>
        /// 将枚举中的所有枚举类型转换成String型，方便写入到枚举Converter中
        /// 按照;分开:对应值
        /// </summary>
        /// <typeparam name="T">枚举类型名称</typeparam>
        /// <returns>Value:Name;Value2:Name2</returns>
        public static string GetEnumsToStr<T>()
            where T : struct
        {
            List<string> results = new List<string>();

            try
            {
                Type t = typeof(T);

                if (!t.IsEnum)
                {
                    return string.Join(";", results);
                }

                FieldInfo[] fieldinfo = t.GetFields(); //获取字段信息对象集合

                foreach (FieldInfo field in fieldinfo)
                {
                    if (!field.IsSpecialName)
                    {
                        string node = (int)field.GetRawConstantValue() + ":";   //获取int数值
                        object[] attrs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attrs.Length > 0)
                        {
                            node += ((DescriptionAttribute)attrs[0]).Description;  //按照描述
                        }
                        else
                        {
                            node += field.Name; //获取文本字段
                        }

                        results.Add(node);
                    }
                }
            }
            catch
            {
                // ignored
            }

            return string.Join(";", results);
        }

        /// <summary>
        /// 将Name值转换成枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型名称</typeparam>
        /// <param name="name">string值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>枚举值</returns>
        public static T ParserEnumByName<T>(string name, T defaultValue)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T必须是枚举类型");
            }

            if (string.IsNullOrEmpty(name))
            {
                return defaultValue;
            }

            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (SafeConverter.SafeToStr(item).ToLower().Equals(name.Trim().ToLower()))
                {
                    return item;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 将value转换成枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型名称</typeparam>
        /// <param name="value">value值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>枚举值</returns>
        public static T ParserEnumByValue<T>(int? value, T defaultValue)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T必须是枚举类型");
            }

            if (value == null)
            {
                return defaultValue;

            }

            try
            {
                T ret = (T)Enum.ToObject(typeof(T), value);
                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return defaultValue;

        }



        /// <summary>
        /// 按照显示名称转换成枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型名称</typeparam>
        /// <param name="displayName">显示名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>枚举值</returns>
        public static T ParserEnumByDisplayName<T>(string displayName, T defaultValue)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T必须是枚举类型");
            }

            if (string.IsNullOrEmpty(displayName))
            {
                return defaultValue;
            }

            foreach (MemberInfo member in typeof(T).GetMembers())
            {
                if (PropertyWrapper.GetDisplayAttributeName(member).Equals(displayName))
                {
                    return ParserEnumByName(member.Name, defaultValue);
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// 按照描述值转换成枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型名称</typeparam>
        /// <param name="displayNameOrDesc">描述值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>枚举值</returns>
        public static T ParserEnumByDisplayNameOrDesc<T>(string displayNameOrDesc, T defaultValue)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T必须是枚举类型");
            }

            if (string.IsNullOrEmpty(displayNameOrDesc))
            {
                return defaultValue;
            }

            foreach (MemberInfo member in typeof(T).GetMembers())
            {
                if (PropertyWrapper.GetDisplayAttributeName(member).Equals(displayNameOrDesc) || PropertyWrapper.GetDisplayAttributeDesc(member).Equals(displayNameOrDesc))
                {
                    return ParserEnumByName(member.Name, defaultValue);
                }
            }

            return defaultValue;
        }
    }
}
