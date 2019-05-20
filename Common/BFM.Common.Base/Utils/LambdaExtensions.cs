using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

namespace BFM.Common.Base.Utils
{

    public static class LambdaExtensions
    {
        /// <summary>
        /// 创建lambda表达式：p=>true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return p => true;
        }

        /// <summary>
        /// 创建lambda表达式：p=>false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return p => false;
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Expression<Func<T, TKey>> GetOrderExpression<T, TKey>(string propertyName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            return Expression.Lambda<Func<T, TKey>>(Expression.Property(parameter, propertyName), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName == propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateEqual<T>(string propertyName, object propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, member.Type);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName != propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateNotEqual<T>(string propertyName, object propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, member.Type);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.NotEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName > propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateGreaterThan<T>(string propertyName, object propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, member.Type);//创建常数

            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName < propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLessThan<T>(string propertyName, object propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, member.Type);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.LessThan(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName >= propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateGreaterThanOrEqual<T>(string propertyName, object propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, member.Type);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName <= propertyValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CreateLessThanOrEqual<T>(string propertyName, object propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");//创建参数p
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            ConstantExpression constant = Expression.Constant(propertyValue, member.Type);//创建常数
            return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(member, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：p=>p.propertyName.Contains(propertyValue)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetContains<T>(string propertyName, object propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：!(p=>p.propertyName.Contains(propertyValue))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> GetNotContains<T>(string propertyName, object propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));
            return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call(member, method, constant)), parameter);
        }

        /// <summary>
        /// 根据 直接 表达式 转换成 Lambda 表达式
        /// </summary>
        /// <typeparam name="T">表达式方法类型</typeparam>
        /// <param name="propertyName">字段名</param>
        /// <param name="propertyValue">比较值</param>
        /// <param name="expression">直接表达式</param>
        /// <returns></returns>
	    public static Expression<Func<T, bool>> GetExpressionByString<T>(string propertyName, object propertyValue,
            string expression)
        {
            expression = expression.Trim().ToLower();
            string value = SafeConverter.SafeToStr(propertyValue);

            //转String
            if ((value.Length > 1) && (value[0] == '\''))
            {
                value = value.Substring(1);

                if ((value.Length > 1) && (value[value.Length - 1] == '\''))
                {
                    value = value.Substring(0, value.Length - 1);
                }

                propertyValue = value;
            }

            //格式化类型
            Type type = typeof(T);
            PropertyInfo propertyInfo = type.GetProperty(propertyName); //获取指定名称的属性
            if (propertyInfo == null)
            {
                throw new Exception("条件中的字段名不存在，请检查。");
            }
            propertyValue = SafeConverter.SafeToObject(propertyInfo.PropertyType, propertyValue);

            string lambadaExpression = $"{propertyName}{expression}@0";
            object[] values = new object[] { propertyValue };
            return System.Linq.Dynamic.DynamicExpression.ParseLambda<T, bool>(lambadaExpression, values);

            #region 获取Lambda表达式 - 之前的方法，已删除

            //if ((expression == "=") || (expression == "=="))
            //{
            //    return CreateEqual<T>(propertyName, propertyValue);
            //}
            //else if (expression == "!=")
            //{
            //    return CreateNotEqual<T>(propertyName, propertyValue);
            //}
            //else if (expression == "like")
            //{
            //    return GetContains<T>(propertyName, propertyValue);
            //}
            //else if (expression == "notlike")
            //{
            //    return GetNotContains<T>(propertyName, propertyValue);
            //}
            //else if (expression == ">")
            //{
            //    return CreateGreaterThan<T>(propertyName, propertyValue);
            //}
            //else if (expression == ">=")
            //{
            //    return CreateGreaterThanOrEqual<T>(propertyName, propertyValue);
            //}
            //else if (expression == "<")
            //{
            //    return CreateLessThan<T>(propertyName, propertyValue);
            //}
            //else if (expression == "<=")
            //{
            //    return CreateLessThanOrEqual<T>(propertyName, propertyValue);
            //}
            //else
            //{
            //    return False<T>();
            //}

            //#endregion 
            //#region 获取Lambda表达式

            //if ((expression == "=") || (expression == "=="))
            //{
            //    return CreateEqual<T>(propertyName, propertyValue);
            //}
            //else if (expression == "!=")
            //{
            //    return CreateNotEqual<T>(propertyName, propertyValue);
            //}
            //else if (expression == "like")
            //{
            //    return GetContains<T>(propertyName, propertyValue);
            //}
            //else if (expression == "notlike")
            //{
            //    return GetNotContains<T>(propertyName, propertyValue);
            //}
            //else if (expression == ">")
            //{
            //    return CreateGreaterThan<T>(propertyName, propertyValue);
            //}
            //else if (expression == ">=")
            //{
            //    return CreateGreaterThanOrEqual<T>(propertyName, propertyValue);
            //}
            //else if (expression == "<")
            //{
            //    return CreateLessThan<T>(propertyName, propertyValue);
            //}
            //else if (expression == "<=")
            //{
            //    return CreateLessThanOrEqual<T>(propertyName, propertyValue);
            //}
            //else
            //{
            //    return False<T>();
            //}

            #endregion
        }

        /// <summary>
                /// 使用 Expression.OrElse 的方式拼接两个 System.Linq.Expression。
                /// </summary>
                /// <typeparam name="T">表达式方法类型</typeparam>
                /// <param name="left">左边的 System.Linq.Expression 。</param>
                /// <param name="right">右边的 System.Linq.Expression。</param>
                /// <returns>拼接完成的 System.Linq.Expression。</returns>
        public static Expression<T> Or<T>(this Expression<T> left, Expression<T> right)
        {
            return MakeBinary(left, right, Expression.OrElse);
        }

        /// <summary>
                /// 使用 Expression.AndAlso 的方式拼接两个 System.Linq.Expression。
                /// </summary>
                /// <typeparam name="T">表达式方法类型</typeparam>
                /// <param name="left">左边的 System.Linq.Expression 。</param>
                /// <param name="right">右边的 System.Linq.Expression。</param>
                /// <returns>拼接完成的 System.Linq.Expression。</returns>
        public static Expression<T> And<T>(this Expression<T> left, Expression<T> right)
        {
            return MakeBinary(left, right, Expression.AndAlso);
        }

        /// <summary>
                /// 使用自定义的方式拼接两个 System.Linq.Expression。
                /// </summary>
                /// <typeparam name="T">表达式方法类型</typeparam>
                /// <param name="left">左边的 System.Linq.Expression 。</param>
                /// <param name="right">右边的 System.Linq.Expression。</param>
                /// <param name="func"> </param>
                /// <returns>拼接完成的 System.Linq.Expression。</returns>
        private static Expression<T> MakeBinary<T>(this Expression<T> left, Expression<T> right, Func<Expression, Expression, Expression> func)
        {
            Debug.Assert(func != null, "func != null");
            return MakeBinary((LambdaExpression)left, right, func) as Expression<T>;
        }

        /// <summary>
                /// 拼接两个 <paramref>
                ///        <name>System.Linq.Expression</name>
                ///      </paramref>  ，两个 <paramref>
                ///                         <name>System.Linq.Expression</name>
                ///                       </paramref>  的参数必须完全相同。
                /// </summary>
                /// <param name="left">左边的 <paramref>
                ///                          <name>System.Linq.Expression</name>
                ///                        </paramref> </param>
                /// <param name="right">右边的 <paramref>
                ///                           <name>System.Linq.Expression</name>
                ///                         </paramref> </param>
                /// <param name="func">表达式拼接的具体逻辑</param>
                /// <returns>拼接完成的 <paramref>
                ///                  <name>System.Linq.Expression</name>
                ///                </paramref> </returns>
        private static LambdaExpression MakeBinary(this LambdaExpression left, LambdaExpression right, Func<Expression, Expression, Expression> func)
        {
            var data = Combinate(right.Parameters, left.Parameters).ToArray();
            right = ParameterReplace.Replace(right, data) as LambdaExpression;
            Debug.Assert(right != null, "right != null");
            return Expression.Lambda(func(left.Body, right.Body), left.Parameters.ToArray());
        }

        private static IEnumerable<KeyValuePair<T, T>> Combinate<T>(IEnumerable<T> left, IEnumerable<T> right)
        {
            var a = left.GetEnumerator();
            var b = right.GetEnumerator();
            while (a.MoveNext() && b.MoveNext())
                yield return new KeyValuePair<T, T>(a.Current, b.Current);
        }
    }

    #region class: ParameterReplace
    internal sealed class ParameterReplace : ExpressionVisitor
    {
        public static Expression Replace(Expression e, IEnumerable<KeyValuePair<ParameterExpression, ParameterExpression>> paramList)
        {
            var item = new ParameterReplace(paramList);
            return item.Visit(e);
        }

        private readonly Dictionary<ParameterExpression, ParameterExpression> _parameters;

        public ParameterReplace(IEnumerable<KeyValuePair<ParameterExpression, ParameterExpression>> paramList)
        {
            _parameters = paramList.ToDictionary(p => p.Key, p => p.Value, new ParameterEquality());
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression result;
            if (_parameters.TryGetValue(p, out result))
                return result;
            return base.VisitParameter(p);
        }

        #region class: ParameterEquality
        private class ParameterEquality : IEqualityComparer<ParameterExpression>
        {
            public bool Equals(ParameterExpression x, ParameterExpression y)
            {
                if (x == null || y == null)
                    return false;

                return x.Type == y.Type;
            }

            public int GetHashCode(ParameterExpression obj)
            {
                if (obj == null)
                    return 0;

                return obj.Type.GetHashCode();
            }
        }
        #endregion
    }
    #endregion
}
