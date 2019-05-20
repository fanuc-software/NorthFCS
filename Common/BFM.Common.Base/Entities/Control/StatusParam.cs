using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XL.CSharp.Commom.Entities.Control
{

	public class StatusParam
	{
		private List<ParamDeatil> _Deatil;
		public List<ParamDeatil> Deatil
		{
			get { return _Deatil; }
		}

		public string Name { get; set; }

		/// <summary>
		/// 初始化设备状态栏参数
		/// </summary>
		/// <param name="name">名称</param>
		public StatusParam(string name)
		{
			this.Name = name;
			this._TotalTime = 0;
			_Deatil = new List<ParamDeatil>();
		}

		private int _TotalTime;
		private readonly int MaxTime = 24 * 12;

		/// <summary>
		/// 添加一条颜色状态，超过MaxTime后不予以添加
		/// </summary>
		/// <param name="time">持续时间，单位：5min</param>
		/// <param name="color">颜色枚举</param>
		public void AddItem(int time, int color = 0)
		{
			if (_TotalTime == MaxTime) return;
			else if ((_TotalTime + time) > MaxTime)
			{
				_Deatil.Add(new ParamDeatil()
				{
					Color = GetColor((ColorEnum)color),
					Value = MaxTime - _TotalTime,
				});
				_TotalTime = MaxTime;
			}
			else if ((_TotalTime + time) <= MaxTime)
			{
				_Deatil.Add(new ParamDeatil()
				{
					Color = GetColor((ColorEnum)color),
					Value = time,
				});
				_TotalTime += time;
			}
		}

		private Color GetColor(ColorEnum _enum)
		{
			switch (_enum)
			{
				case ColorEnum.Alarm:
					return Color.Yellow;
				case ColorEnum.Background:
					return Color.White;
				case ColorEnum.Other:
					return Color.Gray;
				case ColorEnum.Unusual:
					return Color.Red;
				case ColorEnum.Usual:
					return Color.LightGreen;
				default: break;
			}
			return Color.Gray;
		}

		public class ParamDeatil
		{
			public Color Color { get; set; }
			public int Value { get; set; }
		}

		/// <summary>
		/// 状态颜色枚举
		/// </summary>
		public enum ColorEnum
		{
            /// <summary>
            /// 可用 LightGreen
            /// </summary>
			Usual = 1,
            /// <summary>
            /// 不可用 Red
            /// </summary>
			Unusual = 2,
            /// <summary>
            /// 报警 Yellow
            /// </summary>
			Alarm = 3,
            /// <summary>
            /// 背景 White
            /// </summary>
			Background = 4,
            /// <summary>
            /// 其他 Gray
            /// </summary>
			Other = 0
		}

	}
}
