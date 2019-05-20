using System;

namespace BFM.WPF.Base.Command
{
    /// <summary>
    /// Mvvm模式下传递命令
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class RelayCommand<T> : CommandCore
	{
		private readonly Action<T> _execute;
		private readonly Func<T, bool> _canExecute;

		public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
		{
			if (execute == null)
				throw new ArgumentNullException();
			
			if (canExecute == null)
				canExecute = _ => true;

			_execute = execute;
			_canExecute = canExecute;
		}

		public override bool CanExecute(object parameter)
		{
			return _canExecute((T)parameter);
		}

		protected override void OnExecute(object parameter)
		{
			_execute((T)parameter);
		}
	}

	public class RelayCommand : RelayCommand<object>
	{
		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
			: base(execute, canExecute)
		{
			
		}
	}

}
