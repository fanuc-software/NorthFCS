using System;
using System.Windows.Input;

namespace BFM.WPF.Base.Command
{
	public abstract class CommandCore : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public virtual bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			if (CanExecute(parameter) == false)
				return;

			OnExecute(parameter);
		}

		protected abstract void OnExecute(object parameter);

		protected void RaiseCanExecuteChanged()
		{
			OnCanExecuteChanged(EventArgs.Empty);
		}

		protected virtual void OnCanExecuteChanged(EventArgs e)
		{
			if (CanExecuteChanged != null)
				CanExecuteChanged(this, e);
		}
	}
}
