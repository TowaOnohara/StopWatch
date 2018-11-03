using System;
using System.Collections.Generic;
using System.Text;

namespace StopWatchCore.Frameworks.Messengers
{
    public class StartViewMessage : IMessage
    {
        public Type ViewModelType { get; }
        public StartViewMessage(Type viewModelType)
        {
            this.ViewModelType = viewModelType;
        }
    }
}
