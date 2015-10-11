using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.FriendStream
{
    class EventStreamingLikeButton : Interface
    {
        public void Handle(GameClient Session, ClientMessage Message)
        {
            int id = Message.PopWiredInt32();
            int userid = Message.PopWiredInt32();
            Session.GetHabbo().GetStream().LikeStreamEntry(id, userid);
        }
    }
}
