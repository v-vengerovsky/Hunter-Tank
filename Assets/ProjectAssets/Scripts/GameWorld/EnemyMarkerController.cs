using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace HunterTank
{
	public class EnemyMarkerController
	{
		private EnemyMarkerView _view;
		private Camera _camera;

		private Dictionary<int,Image> _notifiers = new Dictionary<int, Image>();
		private Vector3 _playerPos;

		public EnemyMarkerController(EnemyMarkerView view, Camera camera)
		{
			_view = view;
			_camera = camera;
		}

		public void AddNotifier(IPosNotifier notifier)
		{
			_notifiers.Add(notifier.Id, CreateImage());
		}

		public void RemoveNotifier(IPosNotifier notifier)
		{
			GameObject.Destroy(_notifiers[notifier.Id].gameObject);
			_notifiers.Remove(notifier.Id);
		}

		private Image CreateImage()
		{
			GameObject resultGo = new GameObject();
			var result = resultGo.AddComponent<Image>();
			result.transform.SetParent(_view.Panel.transform);
			result.rectTransform.sizeDelta = _view.ArrowSize;
			result.rectTransform.pivot = _view.ArrowPivot;
			result.sprite = _view.Arrow;
			return result;
		}

		public void NotifyPlayerPosition(IPosNotifier notifier, Vector3 position)
		{
			_playerPos = position;
		}

		public void NotifyPosition(IPosNotifier notifier, Vector3 position)
		{
			SetImageTransform(_notifiers[notifier.Id], _playerPos, position);
		}

		private void SetImageTransform(Image image, Vector3 origin, Vector3 target)
		{
			Vector3 viewPortOrigin = _camera.WorldToViewportPoint(origin);
			Vector3 viewPortTarget = _camera.WorldToViewportPoint(target);
			Vector3 imagePos = viewPortTarget - viewPortOrigin;
			imagePos.z = 0f;
			//imagePos.x = Mathf.Clamp01(imagePos.x);
			//imagePos.y = Mathf.Clamp01(imagePos.y);
			//imagePos = imagePos.normalized/2;
			image.gameObject.SetActive(imagePos.magnitude > 1f);

			imagePos = imagePos.normalized/2;

			//imagePos.Normalize();


			Vector3 viewportCenter = new Vector3(0.5f, 0.5f,0f);
			//imagePos += viewportCenter;
			float angle = Vector3.Angle(imagePos, Vector3.right) + _view.ArrowOffsetAngle;

			if (imagePos.y < 0)
			{
				angle = 360f - angle;
			}
			float tempAngle = Math.Min(angle % 90f, 90f - (angle % 90f));
			float magnitude = 1f / Mathf.Cos(tempAngle / 180 * Mathf.PI);
			imagePos = magnitude * imagePos + viewportCenter;
				 
			image.rectTransform.anchorMax = imagePos;
			image.rectTransform.anchorMin = imagePos;
			image.rectTransform.anchoredPosition = Vector2.zero;
			Vector3 eulerAngles = image.rectTransform.localEulerAngles;
			eulerAngles.z = angle;
			image.rectTransform.localEulerAngles = eulerAngles;
		}
	}
}
