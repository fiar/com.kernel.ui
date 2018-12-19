using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kernel.UI
{
	[RequireComponent(typeof(Canvas))]
	public class UIBehaviour : MonoBehaviour
	{
		[SerializeField, Tooltip("Disable childs in containers (Editor only)")]
		private bool _disableChildsInContainersOnStart;
		[Space]
		[SerializeField]
		private string _defaultContainer = "Default";

		private Canvas _canvas;
		private Dictionary<string, RectTransform> _containers;


		protected virtual void Awake()
		{
			_canvas = GetComponent<Canvas>();

			UpdateContainers();

#if UNITY_EDITOR
			if (_disableChildsInContainersOnStart)
			{
				foreach (var kvp in _containers)
				{
					foreach (Transform t in kvp.Value)
					{
						t.gameObject.SetActive(false);
					}
				}
			}
#endif
		}

		public Canvas Canvas
		{
			get { return _canvas; }
		}

		public RectTransform GetContainer(string containerName)
		{
			if (string.IsNullOrEmpty(containerName))
			{
				containerName = _defaultContainer;
			}

			Debug.Assert(_containers.ContainsKey(containerName), "Container not exists: " + containerName + ", in behaviour: " + gameObject.name);

			return _containers.ContainsKey(containerName) ? _containers[containerName] : null;
		}

		protected void UpdateContainers()
		{
			_containers = new Dictionary<string, RectTransform>();
			foreach (var child in transform)
			{
				var container = child as RectTransform;
				Debug.Assert(!_containers.ContainsKey(container.name), "Container already exists: " + container.name);

				if (!_containers.ContainsKey(container.name))
				{
					_containers.Add(container.name, container);
				}
			}
		}
	}
}
