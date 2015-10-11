using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Essential.Communication.Messages.Help
{
    class ModMuteUserMessageEvent : Interface
    {
        public void Handle(HabboHotel.GameClients.GameClient Session, global::Essential.Messages.ClientMessage Event)
        {
            if (!Session.GetHabbo().HasFuse("acc_supporttool"))
			    return;
            uint num = Event.PopWiredUInt();
            string text = Event.PopFixedString();
            int num2 = Event.PopWiredInt32();
            Console.WriteLine(num + "      " + text + "     " + num2);
            string string_ = string.Concat(new object[]
				{
					"User: ",
					num,
					", Message: ",
					text,
                    ", Time: ",
                    num2
				});
            Essential.GetGame().GetModerationTool().MuteUser(Session, num, text, num2);
            Essential.GetGame().GetClientManager().StoreCommand(Session, "ModTool - Mute User", string_);
        }
    }
}
