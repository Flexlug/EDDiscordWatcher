using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscordWatcher.Services.Interfaces
{
    internal interface IEDDNMessagesService
    {
        public delegate void EventMessageHandler(string message);
        public event EventMessageHandler OnMessage;
    }
}
