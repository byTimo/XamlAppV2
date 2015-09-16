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
        /// Получить имя команды
        /// </summary>
        public string Name { get; }

        ///<summary>
        /// Параметризованное действие которое вызывается при активации команды.
        /// </summary>
        public event Action<object> Do;

        /// <summary>
        /// Будевое значение, отвечающие за возможность выполнения команды.
        /// </summary>
        private bool _canExecute;

        public Command(string name)
        {
            _canExecute = true;
            Name = name;
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
            Do?.Invoke(param);
        }
    }
}
