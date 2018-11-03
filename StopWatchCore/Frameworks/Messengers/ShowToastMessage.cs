using System;
using System.Collections.Generic;
using System.Text;

namespace StopWatchCore.Frameworks.Messengers
{
    public class ShowToastMessage : IMessage
    {
        public string Text { get; }
        public ShowToastMessage(string text)
        {
            this.Text = text;
        }
    }
}
