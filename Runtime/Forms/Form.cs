using Kernel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kernel.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	public class Form : View
	{
		#region Events
		public event Action OpenEvent;
		public event Action OpenedEvent;
		public event Action CloseEvent;
		public event Action ClosedEvent;
		#endregion

		[SerializeField]
		private bool _showFader;

		private FormState _state = FormState.Closed;
		private IEnumerator _processRoutine;
		protected CanvasGroup _canvasGroup;

		public FormState State { get { return _state; } }
		public int TweensCount { get { return _tweens.Count; } }
		public bool WasEnabled { get; private set; }

		private List<UITween> _tweens = new List<UITween>();

		private bool _needCloseFader;


		public bool ShowFader
		{
			get { return _showFader; }
			set { _showFader = value; }
		}

		protected virtual void Awake()
		{
			_canvasGroup = GetComponent<CanvasGroup>();
		}

		protected virtual void Start()
		{
		}

		protected virtual void OnEnable()
		{
			_state = FormState.Closed;
		}

		protected virtual void OnDisable()
		{
			_state = FormState.Closed;
		}

		public virtual void Destroy()
		{
			if (gameObject != null) Destroy(gameObject);
		}

		public virtual void OnOpen(params object[] args)
		{
		}

		public virtual void OnOpened()
		{
		}

		public virtual void OnClose()
		{
		}

		public virtual void OnClosed()
		{
		}

		public virtual void Open(params object[] args)
		{
			OpenInternal(true, args);
		}

		public virtual void OpenIgnoreSibling(params object[] args)
		{
			OpenInternal(false, args);
		}

		private void OpenInternal(bool setAsLastSibling, params object[] args)
		{
			if (IsOpen) return;

			WasEnabled = !gameObject.activeSelf;
			gameObject.SetActive(true);
			_canvasGroup.blocksRaycasts = true;

			if (setAsLastSibling)
				transform.SetAsLastSibling();


			if (_showFader)
			{
				_needCloseFader = true;
				DialogFader.FadeIn(this);
			}

			_state = FormState.Opening;
			OnOpen(args);
			if (OpenEvent != null) OpenEvent.Invoke();

			if (_processRoutine != null) StopCoroutine(_processRoutine);
			_processRoutine = OpenAsync();
			StartCoroutine(_processRoutine);
		}

		public virtual void Close()
		{
			Close(false);
		}

		public virtual void Close(bool force)
		{
			if (_needCloseFader)
			{
				DialogFader.FadeOut(this);
			}

			_canvasGroup.blocksRaycasts = false;

			if (force)
			{
				if (_state == FormState.Closed && !gameObject.activeSelf) return;

				OnClose();
				if (CloseEvent != null) CloseEvent();
				OnClosed();
				if (ClosedEvent != null) ClosedEvent.Invoke();

				gameObject.SetActive(false);
			}
			else
			{
				if (IsClose) return;

				_state = FormState.Closing;

				OnClose();
				if (CloseEvent != null) CloseEvent();

				if (_processRoutine != null) StopCoroutine(_processRoutine);
				_processRoutine = CloseAsync();
				StartCoroutine(_processRoutine);
			}
		}

		public bool IsOpen
		{
			get { return isActiveAndEnabled && (State == FormState.Opening || State == FormState.Opened); }
		}

		public bool IsClose
		{
			get { return !isActiveAndEnabled || State == FormState.Closing || State == FormState.Closed; }
		}

		public void RegisterTween(UITween tween)
		{
			if (!_tweens.Contains(tween))
				_tweens.Add(tween);
		}

		private IEnumerator OpenAsync()
		{
			if (_tweens.Count > 0)
			{
				foreach (var tween in _tweens)
				{
					if (tween != null && tween.isActiveAndEnabled && tween.StartAction != TweenStartAction.DoNothing)
					{
						tween.Open();
					}
				}

				foreach (var tween in _tweens)
				{
					if (tween != null && tween.isActiveAndEnabled)
					{
						while (tween.IsTweening) yield return null;
					}
				}
			}

			_state = FormState.Opened;
			_processRoutine = null;
			OnOpened();
			if (OpenedEvent != null) OpenedEvent.Invoke();
		}

		private IEnumerator CloseAsync()
		{
			if (_tweens.Count > 0)
			{
				foreach (var tween in _tweens)
				{
					if (tween != null && tween.isActiveAndEnabled) tween.Close();
				}

				foreach (var tween in _tweens)
				{
					if (tween != null && tween.isActiveAndEnabled)
					{
						while (tween.IsTweening) yield return null;
					}
				}
			}

			_processRoutine = null;
			OnClosed();
			if (ClosedEvent != null) ClosedEvent.Invoke();

			gameObject.SetActive(false);
		}
	}
}
