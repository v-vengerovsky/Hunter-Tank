using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Extentions
{
	public static bool IsMissingReference(this UnityEngine.Object obj)
	{
		return obj.GetInstanceID() != 0 && obj == null;
	}
}
