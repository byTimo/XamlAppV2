using System;
using System.Windows.Input;

namespace ZappChat_v3.Core
{
    /// <summary>
    /// Класс ViewModelCommand – реализующий интерфейс ICommand, вызывает нужную функцию.
    /// </summary>
    public class Command : ICommand
    {
        /// <summary>
        /// Действие(или параметризованное действие) которое вызывается при активации команды.
        /// </summary>
        public event Action Action;

        public event Action<object> ParameterizedAction;

        /// <summary>
        /// Будевое значение, отвечающие за возможность выполнения команды.
        /// </summary>
        private bool _canExecute;

        /// <summary>
        /// Инициализация нового экземпляра класса без параметров <see cref="Command"/>.
        /// </summary>
        /// <param name="action">Действие.</param>
        /// <param name="canExecute">Если установлено в<c>true</c> [can execute] (выполнение разрешено).</param>
        public Command(Action action, bool canExecute = true)
        {
            //  Set the action.
            Action = action;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Инициализация нового экземпляра класса с параметрами <see cref="Command"/> class.
        /// </summary>
        /// <param name="parameterizedAction">Параметризированное действие.</param>
        /// <param name="canExecute"> Если установлено в <c>true</c> [can execute](выполнение разрешено).</param>
        public Command(Action<object> parameterizedAction, bool canExecute = true)
        {
            //  Set the action.
            ParameterizedAction = parameterizedAction;
            _canExecute = canExecute;
        }
        /// <summary>
        /// Инициализация нового экземпляра класса без начального действия
        /// </summary>
        /// <param name="canExecute">Если установлено в <c>true</c> [can execute](выполнение разрешено)</param>
        public Command(bool canExecute = true)
        {
            _canExecute = canExecute;
        }

        /// <summary>
        /// Установка /  получение значения, отвечающего за возможность выполнения команды
        /// </summary>
        /// <value>
        ///     <c>true</c> если выполнение разрешено; если запрещено - <c>false</c>.
        /// </value>
        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                if (_canExecute != value)
                {
                    _canExecute = value;
                    EventHandler canExecuteChanged = CanExecuteChanged;
                    canExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Определяем метод, определющий, что выполнение команды допускается в текущем состоянии
        /// </summary>
        /// <param name="parameter">Этот параметр используется командой.
        ///  Если команда вызывается без использования параметра,
        ///  то этот объект может быть установлен в  null.</param>
        /// <returns>
        /// > если выполнение команды разрешено; если запрещено - false.
        /// </returns>
        bool ICommand.CanExecute(object parameter)
        {
            return _canExecute;
        }

        /// <summary>
        /// Задание метода, который будет вызван при активации команды.
        /// </summary>
        /// <param name="parameter"> Этот параметр используется командой.
        ///  Если команда вызывается без использования параметра,
        ///  то этот объект может быть установлен в  null.</param>
        void ICommand.Execute(object parameter)
        {
            DoExecute(parameter);

        }
        /// <summary>
        ///  Вызывается, когда меняется возможность выполнения команды
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="param">The param.</param>
        public virtual void DoExecute(object param)
        {
            Action?.Invoke();
            ParameterizedAction?.Invoke(param);
        }
    }
}
