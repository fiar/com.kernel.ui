using UnityEngine;
using System.Collections;
using System;

namespace Kernel.UI
{
	public class FormDialog : Form
	{
		#region Events
		public event Action ContentLoadedEvent;
		#endregion

		[SerializeField]
		private RectTransform _dialog;
		[SerializeField]
		private RectTransform _contentContainer;
		[SerializeField]
		private bool _contentAsyncLoading;

		public FormContent Content { get; private set; }
		public bool IsContentLoading { get; private set; }
		public bool IsContentLoaded { get; private set; }

		private string _contentName;

		private Vector2 _oversize;


		protected override void Awake()
		{
			base.Awake();

			_oversize = _dialog.sizeDelta - _contentContainer.sizeDelta;
		}

		public override void Open(params object[] args)
		{
			base.Open(args);

			if (!IsContentLoaded && !IsContentLoading)
			{
				if (string.IsNullOrEmpty(ContentName))
				{
					Debug.LogWarningFormat("{0}, ContentName is empty", GetType());
				}
				else
				{
					IsContentLoading = true;

					StartCoroutine(OpenAsync());
				}
			}
		}

		private IEnumerator OpenAsync()
		{
			FormContent asset = null;

			if (_contentAsyncLoading)
			{
				var request = Resources.LoadAsync<FormContent>("UI/Contents/" + ContentName);
				yield return request;
				asset = request.asset as FormContent;
			}
			else
			{
				asset = Resources.Load<FormContent>("UI/Contents/" + ContentName);
			}

			Debug.Assert(asset != null);

			if (asset != null)
			{
				var content = GameObject.Instantiate<FormContent>(asset);
				content.transform.SetParent(_contentContainer, false);
				content.gameObject.name = string.Format("Content - {0}", ContentName);
				Content = content as FormContent;

				var size = (content.transform as RectTransform).sizeDelta;
				_dialog.sizeDelta = size + _oversize;
				_contentContainer.sizeDelta = size;

				typeof(FormContent)
					.GetProperty("Dialog")
					.SetValue(Content, this, null);

				IsContentLoaded = true;
			}

			IsContentLoading = false;

			if (IsContentLoaded)
			{
				if (ContentLoadedEvent != null)
					ContentLoadedEvent.Invoke();
			}
		}

		public string ContentName
		{
			get { return _contentName; }
			set
			{
				_contentName = value;
				gameObject.name = string.Format("Dialog - {0}", ContentName);
			}
		}
	}
}
