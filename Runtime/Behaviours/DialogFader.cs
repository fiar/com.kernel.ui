using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

namespace Kernel.UI
{
	public abstract class DialogFader : MonoBehaviour
	{
		#region Instance
		private static string _path = "UI/Behaviours/DialogFader";

		public static string Path
		{
			get { return _path; }
			set { _path = value; }
		}

		private static DialogFader _instance;

		private static DialogFader Instance
		{
			get
			{
				if (_instance == null)
				{
					var resource = Resources.Load<DialogFader>(Path);
					Debug.Assert(resource != null, "DialogFader not found at path: " + Path);
					if (resource != null)
					{
						_instance = GameObject.Instantiate<DialogFader>(resource);
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


		public static void FadeIn(Form form, Action completeCallback = null)
		{
			Instance.FadeIn_Internal(form, completeCallback);
		}

		public static void FadeIn(Form form, float duration, Action completeCallback = null)
		{
			Instance.FadeIn_Internal(form, duration, completeCallback);
		}

		public static void FadeOut(Form form, bool force = false, Action completeCallback = null)
		{
			if (IsInstantiated)
			{
				Instance.FadeOut_Internal(form, force, completeCallback);
			}
		}

		public static void FadeOut(Form form, float duration, bool force = false, Action completeCallback = null)
		{
			if (IsInstantiated)
			{
				Instance.FadeOut_Internal(form, duration, force, completeCallback);
			}
		}


		#region Internal
		protected abstract void FadeIn_Internal(Form form, Action completeCallback = null);
		protected abstract void FadeIn_Internal(Form form, float duration, Action completeCallback = null);
		protected abstract void FadeOut_Internal(Form form, bool force = false, Action completeCallback = null);
		protected abstract void FadeOut_Internal(Form form, float duration, bool force = false, Action completeCallback = null);
		#endregion
	}
}
