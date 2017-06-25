using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HunterTank
{
	public class Utils
	{
		public static Vector3 GetRandomPointInsideCloudOfPoints(Vector3[] cloud, int randomSteps = 10)
		{
			var result = cloud[UnityEngine.Random.Range(0, cloud.Length)];

			for (int i = 0; i < randomSteps; i++)
			{
				result = Vector3.Lerp(result, cloud[UnityEngine.Random.Range(0, cloud.Length)], UnityEngine.Random.Range(0f, 1f));
			}

			return result;
		}
	}
}
