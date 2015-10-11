using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Catalog
{
    class OpenGuildPageMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            ServerMessage message = new ServerMessage(Outgoing.SendGuildParts); //Rootkit
            message.AppendInt32(10);
            message.AppendInt32((int)(Session.GetHabbo().OwnedRooms.Count - this.GetMyRoomsGuilds(Session)));
            foreach (RoomData data in Session.GetHabbo().OwnedRooms)
            {
                if (data.GuildId == 0)
                {
                    message.AppendInt32(data.Id);
                    message.AppendString(data.Name);
                    message.AppendBoolean(false);
                }
            }
            message.AppendInt32(5);
            message.AppendInt32(10);
            message.AppendInt32(3);
            message.AppendInt32(4);
            message.AppendInt32(0x19);
            message.AppendInt32(0x11);
            message.AppendInt32(5);
            message.AppendInt32(0x19);
            message.AppendInt32(0x11);
            message.AppendInt32(3);
            message.AppendInt32(0x1d);
            message.AppendInt32(11);
            message.AppendInt32(4);
            message.AppendInt32(0);
            message.AppendInt32(0);
            message.AppendInt32(0);
            Session.SendMessage(message);
            Session.SendMessage(Essential.GetGame().GetCatalog().groupsDataMessage);
        }
        public int GetMyRoomsGuilds(GameClient Session)
        {
            int num = 0;
            foreach (RoomData data in Session.GetHabbo().OwnedRooms)
            {
                if (data.GuildId != 0)
                {
                    num++;
                }
            }
            return num;
        }
    }
}
