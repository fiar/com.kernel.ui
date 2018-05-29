using System;
using UnityEngine;

namespace Kernel.UI
{
	[RequireComponent(typeof(Canvas))]
	public class UIBehaviour : MonoBehaviour
	{
		[SerializeField, Tooltip("Disable childs in Editor only")]
		private bool _disableChildsOnStart;
		[Space]
		[SerializeField]
		private RectTransform _container;

		private Canvas _canvas;


		protected void Awake()
		{
			_canvas = GetComponent<Canvas>();

#if UNITY_EDITOR
			if (_disableChildsOnStart)
			{
				foreach (Transform t in transform)
				{
					t.gameObject.SetActive(false);
				}
			}
#endif
		}

		public Canvas Canvas
		{
			get { return _canvas; }
		}

		public RectTransform Container
		{
			get { return _container; }
		}

#if UNITY_EDITOR
		protected void Reset()
		{
			if (_container == null)
				_container = transform as RectTransform;
		}
#endif
	}
}
