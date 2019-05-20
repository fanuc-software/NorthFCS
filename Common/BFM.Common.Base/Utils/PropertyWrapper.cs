using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace BFM.Common.Base.Utils
{
    public class PropertyWrapper
    {
        public static string GetDisplayAttributeName(MemberInfo member)
        {
            if (member == null) return string.Empty;

            DisplayAttribute displayAttr =
                (DisplayAttribute) member.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault();
            if (displayAttr == null) return string.Empty;

            return displayAttr.Name;
        }

        public static string GetDisplayAttributeDesc(MemberInfo member)
        {
            if (member == null) return string.Empty;

            DisplayAttribute displayAttr =
                (DisplayAttribute)member.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault();
            if (displayAttr == null) return string.Empty;

            return displayAttr.Description;
        }
    }
}
