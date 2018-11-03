using System;
using System.Collections.Generic;
using System.Text;

namespace StopWatchCore.Frameworks.Messengers
{
    public class Messenger : IDisposable
    {
        private readonly IDictionary<string, Action<IMessage>> _registerMap = new Dictionary<string, Action<IMessage>>();

        public void Send(IMessage message)
        {
            var className = message.GetType().Name;
            if (_registerMap.ContainsKey(className))
            {
                _registerMap[className]?.Invoke(message);
            }
        }

        public void Register(string messageClassName, Action<IMessage> action)
        {
            _registerMap.Add(messageClassName, action);
        }

        public void Dispose()
        {
            _registerMap.Clear();
        }
    }
}
