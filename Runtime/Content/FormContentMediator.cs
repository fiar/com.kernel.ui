using Kernel.Core;
using System;
using UnityEngine;

namespace Kernel.UI
{
	public abstract class FormContentMediator<TView> : FormContentMediator<FormDialog, TView> where TView : FormContent
	{
	}

	public abstract class FormContentMediator<TForm, TView> : Mediator<TView> where TForm : FormDialog where TView : FormContent
	{
		public TForm Form { get; private set; }

		private bool _initialized = false;


		protected override void Start()
		{
			base.Start();

			if (!_initialized) Initialize();
		}

		protected virtual void OnEnable()
		{
			Initialize();
		}

		private void Initialize()
		{
			Form = GetComponentInParent<TForm>();
			View = GetComponent<TView>();

			if (Form != null)
			{
				Form.ContentLoadedEvent += OnContentLoaded;
				Form.OpenEvent += OnFormOpenInternal;
				Form.OpenedEvent += OnFormOpened;
				Form.CloseEvent += OnFormClose;
				Form.ClosedEvent += OnFormClosed;

				if (Form.IsOpen)
				{
					OnFormOpenInternal();
					if (Form.State == FormState.Opened) OnFormOpened();
				}

				_initialized = true;
			}
		}

		protected virtual void OnDisable()
		{
			if (Form != null)
			{
				Form.ContentLoadedEvent -= OnContentLoaded;
				Form.OpenEvent -= OnFormOpenInternal;
				Form.OpenedEvent -= OnFormOpened;
				Form.CloseEvent -= OnFormClose;
				Form.ClosedEvent -= OnFormClosed;
			}
		}

		private void OnContentLoaded()
		{
		}

		protected virtual void OnFormOpenInternal()
		{
			if (!IsRegistered)
			{
				OnRegistered();
				IsRegistered = true;
			}

			OnFormOpen();
		}

		protected virtual void OnFormOpen()
		{
		}

		protected virtual void OnFormOpened()
		{
		}

		protected virtual void OnFormClose()
		{
		}

		protected virtual void OnFormClosed()
		{
		}
	}
}
