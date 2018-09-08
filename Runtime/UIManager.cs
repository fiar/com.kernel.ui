using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kernel.UI
{
	public class UIManager
	{
		#region Instance
		private static UIManager _instance;

		public static UIManager Instance
		{
			get
			{
				if (_instance == null) _instance = new UIManager();
				return _instance;
			}
		}

		public static bool IsInstantiated
		{
			get
			{
				return (_instance != null);
			}
		}
		#endregion

		private static string _path = "UI/Forms";

		public static string Path
		{
			get { return _path; }
			set { _path = value; }
		}

		private Dictionary<string, UIBehaviour> _behaviours;
		private UIBehaviour _defaultBehaviour;

		private List<Form> _forms;


		public static void Initialize()
		{
			Instance.Initialize_Internal();
		}

		public static void Destroy()
		{
			if (!IsInstantiated) return;
			Instance.Destroy_Internal();
		}

		public static Form CreateForm(string formName, Transform parent = null)
		{
			if (!IsInstantiated) return null;
			return Instance.CreateForm_Internal(formName, Instance._defaultBehaviour, parent);
		}

		public static Form CreateForm(string formName, string behaviourName, Transform parent = null)
		{
			if (!IsInstantiated) return null;
			if (!_instance._behaviours.ContainsKey(behaviourName))
			{
				Debug.LogWarningFormat("UIBehaviour with name \"{0}\" not exists", behaviourName);
				behaviourName = Instance._defaultBehaviour.name;
			}
			return Instance.CreateForm_Internal(formName, _instance._behaviours[behaviourName], parent);
		}


		#region Internal
		private void Initialize_Internal()
		{
			var behaviours = GameObject.FindObjectsOfType<UIBehaviour>();
			Debug.Assert(behaviours.Length > 0, "UIBehaviours not found");

			_behaviours = new Dictionary<string, UIBehaviour>();
			foreach (var behaviour in behaviours)
			{
				if (_behaviours.ContainsKey(behaviour.name))
				{
					Debug.LogErrorFormat("UIBehaviour with name \"{0}\" already exists", behaviour.name);
					continue;
				}
				_behaviours.Add(behaviour.name, behaviour);
			}

			_defaultBehaviour = behaviours[0];

			_forms = new List<Form>();

			Canvas.ForceUpdateCanvases();
		}

		private void Destroy_Internal()
		{
			if (_forms != null)
			{
				foreach (var form in _forms)
				{
					if (form != null)
					{
						form.Destroy();
					}
				}
				_forms.Clear();
			}
		}

		private Form CreateForm_Internal(string name, UIBehaviour behaviour, Transform parent)
		{
			var resource = Resources.Load<Form>(Path + "/" + name);
			Debug.Assert(resource != null, "Form (" + name + ") not found");

			var form = GameObject.Instantiate<Form>(resource);
			form.name = name;
			form.transform.SetParent((parent == null) ? behaviour.Container : parent, false);
			form.gameObject.SetActive(false);
			_forms.Add(form);

			return form;
		}
		#endregion
	}
}
