using System;

namespace BFM.WPF.Base.Command
{
	public class PredicateCommand<T, S> : CommandCore
	{
		private readonly Func<T, S> _execute;
		private readonly Func<T, bool> _canExecute;

		public PredicateCommand(Func<T, S> execute, Func<T, bool> canExecute = null)
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

	public class PredicateCommand : PredicateCommand<object, bool>
	{
		public PredicateCommand(Func<object, bool> execute, Func<object, bool> canExecute = null)
			: base(execute, canExecute)
		{

		}
	}
}
