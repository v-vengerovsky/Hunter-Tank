using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HunterTank
{
	public class SpawnVolume : MonoBehaviour
	{
		[SerializeField]
		private MeshFilter _mesh;

		public T Spawn<T>(T original) where T : Component
		{
			var result = Instantiate<T>(original);

			var points = new Vector3[_mesh.sharedMesh.vertices.Length];

			for (int i = 0; i < _mesh.sharedMesh.vertices.Length; i++)
			{
				points[i] = transform.TransformPoint(_mesh.sharedMesh.vertices[i]);
			}

			result.transform.position = Utils.GetRandomPointInsideCloudOfPoints(points);

			return result;
		}

		private void Reset()
		{
			_mesh = GetComponent<MeshFilter>();
		}
	}
}