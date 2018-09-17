using UnityEngine;
using System.Collections;
using Kernel.Core;

namespace Kernel.UI
{
	public abstract class FormContent : View
	{
		public FormDialog Dialog { get; private set; }

		public virtual void OnOpen(params object[] args) { }
	}
}
