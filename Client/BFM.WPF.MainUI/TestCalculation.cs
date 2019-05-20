
using System;

using System.Text;

using System.Collections.Generic;

using BFM.Common.Base;



public class C00b7aa658cf64730a5238039407f97c6

{

public static string AutoCalculation1(string param0)

{

string[] values = param0.Split('|');



int temp = values.Length;

if (temp > 0)

{



if (values[0] == "1")

{

return "1";

}

else if (values[0] == "0")

{

return "0";

}

}

return "-1";

}

}

