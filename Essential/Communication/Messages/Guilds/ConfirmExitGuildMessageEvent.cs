using Essential.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Guilds
{
    class ConfirmExitGuildMessageEvent : Interface
    {
        public void Handle(HabboHotel.GameClients.GameClient Session, global::Essential.Messages.ClientMessage Event)
        {
             int guildId = Event.PopWiredInt32();
            GroupsManager guild = Groups.GetGroupById(guildId);
            int UserId = Event.PopWiredInt32();
            if (UserId != Session.GetHabbo().Id && !guild.UserWithRanks.Contains((int)Session.GetHabbo().Id))
                return;
            if (guild != null)
            {
                ServerMessage Message = new ServerMessage(Outgoing.ConfirmLeaveGroup);
                Message.AppendInt32(UserId);
                Message.AppendInt32(guildId);
                Session.SendMessage(Message);
            }
        }
    }
}
