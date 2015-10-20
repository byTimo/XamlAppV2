using System;

namespace ZappChat_v3.Core
{
    public class OpenCommand : Command
    {
        public event Action<object[]> DoWhenClose;
        public OpenCommand(string name) : base(name) { }

        public void DoWhenCloseExecute(params object[] param)
        {
            DoWhenClose?.Invoke(param);
        }
    }
}