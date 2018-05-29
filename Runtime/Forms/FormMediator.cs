using Kernel.Core;
using System;
using UnityEngine;

namespace Kernel.UI
{
	public abstract class FormMediator<T> : Mediator<T> where T : Form
	{

		protected virtual void OnEnable()
		{
			View.OpenEvent += OnFormOpenInternal;
			View.OpenedEvent += OnFormOpened;
			View.CloseEvent += OnFormClose;
			View.ClosedEvent += OnFormClosed;
		}

		protected virtual void OnDisable()
		{
			if (View != null)
			{
				View.OpenEvent -= OnFormOpenInternal;
				View.OpenedEvent -= OnFormOpened;
				View.CloseEvent -= OnFormClose;
				View.ClosedEvent -= OnFormClosed;
			}
		}

		private void OnFormOpenInternal()
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
