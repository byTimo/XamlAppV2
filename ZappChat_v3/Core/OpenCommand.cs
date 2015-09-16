using System;

namespace ZappChat_v3.Core
{
    public class OpenCommand : Command
    {
        public event Action<object> DoWhenClose;
        public OpenCommand(string name) : base(name) { }

        public void DoExecute(object param, bool isOpened)
        {
            if (isOpened)
                DoWhenClose?.Invoke(param);
            else
                base.DoExecute(param);
        }
    }
}