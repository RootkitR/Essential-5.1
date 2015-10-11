using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Essential.Communication.Messages.FriendStream
{
    class SetEventStreamingAllowedComposer : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
        }
    }
}