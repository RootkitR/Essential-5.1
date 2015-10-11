using Essential.HabboHotel.GameClients;
using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guilds
{
    class SendFurniGuildMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            ServerMessage message = new ServerMessage(Outgoing.SendHtmlColors);
            message.AppendInt32(Session.GetHabbo().dataTable_0.Rows.Count);
            foreach (DataRow dr in Session.GetHabbo().dataTable_0.Rows)
            {
                int num = (int)dr["groupid"];
                GroupsManager guild = Groups.GetGroupById(num);
                message.AppendInt32(guild.Id);
                message.AppendString(guild.Name);
                message.AppendString(guild.Badge);
                message.AppendString(guild.ColorOne);
                message.AppendString(guild.ColorTwo);
                message.AppendBoolean(guild.Id == Session.GetHabbo().FavouriteGroup);
            }
            Session.SendMessage(message);
        }
    }
}
