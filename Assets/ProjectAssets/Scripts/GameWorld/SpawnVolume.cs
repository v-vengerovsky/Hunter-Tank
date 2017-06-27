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

			result.transform.position = CorrectPosition(Utils.GetRandomPointInsideCloudOfPoints(points));

			return result;
		}

		private Vector3 CorrectPosition(Vector3 position)
		{
			Ray ray = new Ray(position, Vector3.down);
			RaycastHit hit;
			//Debug.DrawRay(position, Vector3.down*50f,Color.red,10f);
			if (Physics.Raycast(ray, out hit, 50f,~LayerMask.NameToLayer("Ground")))
			{
				return hit.point;
			}
			else
			{
				return position;
			}
		}

		private void Reset()
		{
			_mesh = GetComponent<MeshFilter>();
		}
	}
}