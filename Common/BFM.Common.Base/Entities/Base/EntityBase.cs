namespace BFM.Common.Data.Entities.Base
{
	/// <summary>
	/// 所有自定义实体的基础接口
	/// </summary>
	public interface IEntityBase
	{
	}

	/// <summary>
	/// 接口：生成JSON时忽略NULL对象
	/// </summary>
	public interface IJsonIgnoreNull : IEntityBase
	{

	}

	/// <summary>
	/// 生成JSON时忽略NULL对象
	/// </summary>
	public class JsonIgnoreNull : IJsonIgnoreNull
	{

	}

	/// <summary>
	/// 接口：类中有枚举在序列化的时候，需要转成字符串
	/// </summary>
	public interface IJsonEnumString : IEntityBase
	{

	}
}
