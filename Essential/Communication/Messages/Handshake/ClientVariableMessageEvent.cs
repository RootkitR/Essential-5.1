using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Handshake
{
    internal sealed class ClientVariableMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            Event.PopWiredInt32();
            string str = Event.PopFixedString();
            string str2 = Event.PopFixedString();
            Session.ClientVar = str;
           // Console.WriteLine(Session.ClientVar);
        }
    }
}
