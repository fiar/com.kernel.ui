using System;
using System.Collections;
using UnityEngine;

namespace Kernel.UI
{
	public abstract class UITween : MonoBehaviour
	{
		#region Events
		public event Action<UITween> OpenedEvent;
		public event Action<UITween> ClosedEvent;
		#endregion

		[SerializeField]
		private Form _form;
		[SerializeField]
		private bool _autoRegister = true;

		[SerializeField]
		private TweenStartAction _startAction = TweenStartAction.StartTween;

		private UITweenState _state = UITweenState.Closed;

		public UITweenState State { get { return _state; } }
		public TweenStartAction StartAction { get { return _startAction; } }

		public Form Form { get { return _form; } }
		public bool WasEnabled { get { return _form.WasEnabled; } }
		public bool IsTweening { get { return _state == UITweenState.Opening || _state == UITweenState.Closing; } }

		protected object _tweenTarget = new object();


		protected void Awake()
		{
			if (_autoRegister)
			{
				if (_form == null)
				{
					Debug.LogWarning("Form is not set. Tween: " + this, this);
					_form = GetComponentInParent<Form>();
				}

				Debug.Assert(_form != null, "Form not found. Tween: " + this, this);
				if (_form != null)
				{
					_form.RegisterTween(this);
				}
			}

			OnAwake();
		}

		public void Open()
		{
			_state = UITweenState.Opening;
			OnOpen();
		}

		public void Close()
		{
			_state = UITweenState.Closing;
			OnClose();
		}

		public virtual void ResetTween()
		{
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnOpen()
		{
		}

		protected virtual void OnClose()
		{
		}

		protected void OpenedEventInvoke()
		{
			_state = UITweenState.Opened;
			if (OpenedEvent != null)
				OpenedEvent.Invoke(this);
		}

		protected void ClosedEventInvoke()
		{
			_state = UITweenState.Closed;
			if (ClosedEvent != null)
				ClosedEvent.Invoke(this);
		}

#if UNITY_EDITOR
		protected void Reset()
		{
			_form = GetComponentInParent<Form>();
		}

		protected void OnValidate()
		{
			if (_form == null)
				_form = GetComponentInParent<Form>();
		}
#endif
	}
}
