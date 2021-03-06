﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BFM.Common.Data.Entities.Base
{
	/// <summary>
	/// 用于实现INotifyPropertyChanged
	/// </summary>
	[Serializable]
	public abstract class BindableBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (this.PropertyChanged != null)
			{

				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

			}
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));//需要VS2015+或新编译器支持
		}

		/// <summary>
		/// 设置属性
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="storage"></param>
		/// <param name="value"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
		{
			if (object.Equals(storage, value)) return false;

			storage = value;
			this.OnPropertyChanged(propertyName);
			return true;
		}
	}
}
