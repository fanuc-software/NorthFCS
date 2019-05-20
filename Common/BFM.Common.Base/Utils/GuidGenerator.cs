using System;

namespace BFM.Common.Base.Utils
{
    public class GuidGenerator
    {
        public static string GetShortGuid()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks).ToUpper();
        }

    }
}
