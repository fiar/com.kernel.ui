using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Kernel.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public abstract class ScreenFader : MonoBehaviour
	{
		#region Instance
		private static string _path = "UI/Behaviours/ScreenFader";

		public static string Path
		{
			get { return _path; }
			set { _path = value; }
		}

		private static ScreenFader _instance;

		private static ScreenFader Instance
		{
			get
			{
				if (_instance == null)
				{
					var resource = Resources.Load<ScreenFader>(Path);
					Debug.Assert(resource != null, "ScreenFader not found at path: " + Path);
					if (resource != null)
					{
						_instance = GameObject.Instantiate<ScreenFader>(resource);
						_instance.name = "~" + _instance.GetType().Name;
						GameObject.DontDestroyOnLoad(_instance);
					}
				}
				return _instance;
			}
		}

		private static bool IsInstantiated
		{
			get { return _instance != null; }
		}
		#endregion


		public static void FadeIn(Action completeCallback = null)
		{
			Instance.FadeIn_Internal(completeCallback);
		}

		public static void FadeIn(float duration, Action completeCallback = null)
		{
			Instance.FadeIn_Internal(duration, completeCallback);
		}

		public static void FadeOut(Action completeCallback = null)
		{
			if (IsInstantiated)
			{
				Instance.FadeOut_Internal(completeCallback);
			}
		}

		public static void FadeOut(float duration, Action completeCallback = null)
		{
			if (IsInstantiated)
			{
				Instance.FadeOut_Internal(duration, completeCallback);
			}
		}

		#region Internal
		protected abstract void FadeIn_Internal(Action completeCallback = null);
		protected abstract void FadeIn_Internal(float duration, Action completeCallback = null);
		protected abstract void FadeOut_Internal(Action completeCallback = null);
		protected abstract void FadeOut_Internal(float duration, Action completeCallback = null);
		#endregion
	}
}
