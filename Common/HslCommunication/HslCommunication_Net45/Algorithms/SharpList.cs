﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HslCommunication.Core;

namespace HslCommunication.Algorithms
{
    /// <summary>
    /// 一个高效的数组管理类，用于高效控制固定长度的数组实现
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    public class SharpList<T>
    {
        #region Constructor

        public SharpList( int count, bool appendLast = false )
        {
            if (count > 8192)
            {
                capacity = 4096;
            }

            array = new T[capacity + count];
            hybirdLock = new SimpleHybirdLock( );
            this.count = count;
            if (appendLast) this.lastIndex = count - 1;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 获取数据的个数
        /// </summary>
        public int Count
        {
            get
            {
                return count;
            }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 新增一个数据值
        /// </summary>
        /// <param name="value">数据值</param>
        public void Add( T value )
        {
            hybirdLock.Enter( );

            if(lastIndex < (capacity + count - 1))
            {
                array[lastIndex++] = value;
            }
            else
            {
                // 需要重新挪位置了
                T[] buffer = new T[capacity + count];
                Array.Copy( array, capacity, buffer, 0, count );
                array = buffer;
            }

            hybirdLock.Leave( );
        }

        public void Add( IEnumerable<T> values )
        {
            foreach(var m in values)
            {
                Add( m );
            }
        }

        /// <summary>
        /// 获取数据的数组值
        /// </summary>
        /// <returns>数组值</returns>
        public T[] ToArray( )
        {
            T[] result = null;
            hybirdLock.Enter( );

            result = new T[count];
            if (lastIndex < count)
            {
                Array.Copy( array, 0, result, 0, lastIndex + 1 );
            }
            else
            {
                Array.Copy( array, lastIndex - count + 1, result, 0, count );
            }
            hybirdLock.Leave( );
            return result;
        }

        /// <summary>
        /// 获取或设置指定索引的位置的数据
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <returns>数据值</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0) throw new IndexOutOfRangeException( "Index must larger than zero" );
                if (index >= count) throw new IndexOutOfRangeException( "Index must smaller than array length" );
                T tmp = default( T );
                hybirdLock.Enter( );

                if (lastIndex < count)
                {
                    tmp = array[index];
                }
                else
                {
                    tmp = array[index + lastIndex - count + 1];
                }

                hybirdLock.Leave( );
                return tmp;
            }
            set
            {
                if (index < 0) throw new IndexOutOfRangeException( "Index must larger than zero" );
                if (index >= count) throw new IndexOutOfRangeException( "Index must smaller than array length" );
                hybirdLock.Enter( );

                if (lastIndex < count)
                {
                    array[index] = value;
                }
                else
                {
                    array[index + lastIndex - count + 1] = value;
                }

                hybirdLock.Leave( );
            }
        }

        #endregion

        #region private Member

        private T[] array;
        private int capacity = 2048;          // 整个数组的附加容量
        private int count = 0;                // 数组的实际数据容量
        private int lastIndex = 0;            // 最后一个数的索引位置
        private SimpleHybirdLock hybirdLock;  // 数组的操作锁

        #endregion
    }
}
