using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using SharpSentinel.UI.Extensions;
using Action = System.Action;

namespace SharpSentinel.UI.Common
{
    public class FluidCommand : PropertyChangedBase, ICommand
    {
        private readonly Func<CancellationToken, Task> _execute;
        private CancellationTokenSource _cancellationTokenSource;

        private readonly Func<bool> _canExecute;
        private readonly bool _noCursor;

        private bool _isExecuting;
        public bool IsExecuting
        {
            get { return _isExecuting; }
            private set
            {
                if (this.Set(ref this._isExecuting, value))
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
        public FluidCommand CancelCommand { get; }

        private FluidCommand(Func<CancellationToken, Task> execute,
            Func<bool> canExecute = null,
            bool noCursor = false,
            bool withCancelCommand = true)
        {
            this._execute = this.Wrap(execute);
            this._canExecute = canExecute;
            this._noCursor = noCursor;

            if (withCancelCommand)
#pragma warning disable 1998
                this.CancelCommand = new FluidCommand(async _ => this.Cancel(), this.CanCancel, withCancelCommand: false);
#pragma warning restore 1998
        }
        
        /// <summary>
        /// Wrap the task with a cancelcationtoken
        /// </summary>
        private Func<CancellationToken, Task> Wrap(Func<CancellationToken, Task> execute)
        {
            return async cancellationToken =>
            {
                try
                {
                    if (this._noCursor == false)
                        Mouse.OverrideCursor = Cursors.AppStarting;

                    await execute(cancellationToken);
                }
                catch (TaskCanceledException)
                {

                }
                catch (Exception)
                {

                }
                finally
                {
                    Mouse.OverrideCursor = null;
                }
            };
        }


        public static FluidCommandBuilder Async(Func<Task> execute)
        {
            return Async(_ => execute());
        }

        public static FluidCommandBuilder Async(Func<CancellationToken, Task> execute)
        {
            return new FluidCommandBuilder(execute);
        }

        public static FluidCommandBuilder Sync(Action execute)
        {
#pragma warning disable 1998
            return Async(async () => execute());
#pragma warning restore 1998
        }

        /// <summary>
        /// Sync execute
        /// </summary>
        public void Execute()
        {
#pragma warning disable 4014
            this.ExecuteAsync();
#pragma warning restore 4014
        }

        /// <summary>
        /// Async awaitable execute
        /// </summary>
        public async Task ExecuteAsync()
        {
            if (((ICommand)this).CanExecute(null) == false)
                return;

            try
            {
                this.IsExecuting = true;
                this._cancellationTokenSource = new CancellationTokenSource();

                await this._execute(this._cancellationTokenSource.Token);
            }
            finally
            {
                this.IsExecuting = false;
                this._cancellationTokenSource = null;
            }
        }

        #region Cancel

        private bool CanCancel()
        {
            return this._cancellationTokenSource != null;
        }

        private void Cancel()
        {
            this._cancellationTokenSource.Cancel();
        }

        #endregion

        #region ICommand

        /// <summary>
        /// Implementation of ICommand.CanExecute
        /// </summary>
        /// <param name="parameter">This parameter will be ignored</param>
        bool ICommand.CanExecute(object parameter)
        {
            if (this.IsExecuting)
                return false;

            return this._canExecute?.Invoke() ?? true;
        }

        /// <summary>
        /// Implementation of ICommand.Execute
        /// </summary>
        /// <param name="parameter">This paramater will be ignored</param>
        void ICommand.Execute(object parameter)
        {
            this.Execute();
        }

        /// <summary>
        /// Update canExecute
        /// </summary>
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #endregion

        #region Builder

        public class FluidCommandBuilder
        {
            private readonly Func<CancellationToken, Task> _execute;
            private Func<bool> _canExecute;
            private bool _noCursor;

            public FluidCommandBuilder(Func<CancellationToken, Task> execute)
            {
                this._execute = execute;
            }

            public FluidCommandBuilder CanExecute(Func<bool> canExecute)
            {
                this._canExecute = canExecute;
                return this;
            }

            public FluidCommandBuilder NoCursor()
            {
                this._noCursor = true;
                return this;
            }

            public FluidCommand Create()
            {
                return new FluidCommand(this._execute, this._canExecute, this._noCursor);
            }

            public static implicit operator FluidCommand(FluidCommandBuilder builder)
            {
                return builder.Create();
            }
        }

        #endregion
    }
}