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

		private UIBehaviour _behaviour;

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

		public static Form CreateForm(string formName, string containerName = "")
		{
			if (!IsInstantiated) return null;
			return Instance.CreateForm_Internal(formName, Instance._behaviour, containerName);
		}

		public static Form CreateForm(string formName, Transform parent)
		{
			if (!IsInstantiated) return null;
			if (parent == null) return CreateForm(formName);
			return Instance.CreateForm_Internal(formName, parent);
		}

		public static T CreateForm<T>(string formName, string containerName = "") where T : Form
		{
			return CreateForm(formName, containerName) as T;
		}

		public static T CreateForm<T>(string formName, Transform parent) where T : Form
		{
			return CreateForm(formName, parent) as T;
		}


		#region Internal
		private void Initialize_Internal()
		{
			var behaviours = GameObject.FindObjectsOfType<UIBehaviour>();
			Debug.Assert(behaviours.Length == 1, "UIBehaviour must be in single instance! Found: " + behaviours.Length);
			if (behaviours.Length == 0) return;

			_behaviour = behaviours[0];

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

		private Form CreateForm_Internal(string name, UIBehaviour behaviour, string containerName)
		{
			var resource = Resources.Load<Form>(Path + "/" + name);
			Debug.Assert(resource != null, "Form (" + name + ") not found");

			var container = behaviour.GetContainer(containerName);
			Debug.Assert(container != null, "Container not exists: " + containerName);

			if (container != null)
			{
				var form = GameObject.Instantiate<Form>(resource);
				form.name = name;
				form.transform.SetParent(container, false);
				form.gameObject.SetActive(false);
				_forms.Add(form);
				return form;
			}

			return null;
		}

		private Form CreateForm_Internal(string name, Transform parent)
		{
			var resource = Resources.Load<Form>(Path + "/" + name);
			Debug.Assert(resource != null, "Form (" + name + ") not found");

			var form = GameObject.Instantiate<Form>(resource);
			form.name = name;
			form.transform.SetParent(parent, false);
			form.gameObject.SetActive(false);
			_forms.Add(form);

			return form;
		}
		#endregion
	}
}
