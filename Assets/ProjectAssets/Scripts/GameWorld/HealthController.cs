using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using IEnemyPosNotifiable = HunterTank.IEnemyNotifiable<HunterTank.IPlayerNotifier>;

namespace HunterTank
{
	public class HealthController: IEnemyPosNotifiable
	{
		private HealthView _view;
		private Camera _camera;

		private Dictionary<int, Image> _notifiers = new Dictionary<int, Image>();

		public HealthController(HealthView view, Camera camera)
		{
			_view = view;
			_camera = camera;
		}
		public void OnSpawn(IPlayerNotifier enemy)
		{
			AddNotifier(enemy);
		}

		public void OnDestroy(IPlayerNotifier enemy, ICollidable other)
		{
			RemoveNotifier(enemy);
		}

		public void Notify(IPlayerNotifier enemy)
		{
			NotifyEnemyPosition(enemy, enemy.Position);
		}

		private void NotifyEnemyPosition(IPlayerNotifier notifier, Vector3 position)
		{
			SetImageTransform(_notifiers[notifier.Id], position, notifier.RangeFloat.Value/ notifier.RangeFloat.Max);
		}

		private void AddNotifier(IPlayerNotifier notifier)
		{
			_notifiers.Add(notifier.Id, CreateImage());
		}

		private void RemoveNotifier(IPlayerNotifier notifier)
		{
			GameObject.Destroy(_notifiers[notifier.Id].gameObject);
			_notifiers.Remove(notifier.Id);
		}

		private Image CreateImage()
		{
			GameObject resultGo = new GameObject();
			var result = resultGo.AddComponent<Image>();
			result.transform.SetParent(_view.Panel.transform);
			result.rectTransform.pivot = _view.BarPivot;
			result.rectTransform.anchoredPosition = _view.Offset;
			result.gameObject.SetActive(false);
			return result;
		}

		private void SetImageTransform(Image image, Vector3 target, float fill)
		{
			Vector3 viewPortTarget = _camera.WorldToViewportPoint(target);
			Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 0f);
			Vector3 imagePos = viewPortTarget;// - viewportCenter;
			viewPortTarget.z = 0f;
			image.gameObject.SetActive(viewPortTarget.magnitude < 1f);

			//imagePos = imagePos.normalized / 2+ viewportCenter;

			image.rectTransform.anchorMax = viewPortTarget;
			image.rectTransform.anchorMin = viewPortTarget;
			//image.rectTransform.anchoredPosition = _view.Offset;

			image.rectTransform.sizeDelta = _view.GetSize(fill);
			image.color = _view.GetColor(fill);
		}
	}
}
