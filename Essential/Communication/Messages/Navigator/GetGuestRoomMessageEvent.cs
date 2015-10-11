using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
namespace Essential.Communication.Messages.Navigator
{
	internal sealed class GetGuestRoomMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			uint uint_ = Event.PopWiredUInt();
			bool bool_ = Event.PopWiredBoolean();
			bool flag = Event.PopWiredBoolean();

            if (uint_ == Session.GetHabbo().CurrentRoomId)
            {
                return;
            }
           
            RoomData @class = Essential.GetGame().GetRoomManager().method_12(uint_);
			if (@class != null)
			{
                ServerMessage Message = new ServerMessage(Outgoing.GetGuestRoomResult); // Updated
                Message.AppendBoolean(false);
                Message.AppendUInt(@class.Id);
                Message.AppendBoolean(false);
                Message.AppendString(@class.Name);
                Message.AppendBoolean(true);
                Message.AppendInt32(@class.OwnerId);
                Message.AppendStringWithBreak(@class.Owner);
                Message.AppendInt32(@class.State); // @class state
                Message.AppendInt32(@class.UsersNow);
                Message.AppendInt32(@class.UsersMax);
                Message.AppendStringWithBreak(@class.Description);
                Message.AppendInt32(0); // dunno!
                Message.AppendInt32((@class.Category == 9) ? 2 : 0); // can trade!
                Message.AppendInt32(@class.Score);
                Message.AppendInt32(@class.Category);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendStringWithBreak("");
                Message.AppendInt32(@class.TagCount);

                foreach (string Tag in @class.Tags)
                {
                    Message.AppendStringWithBreak(Tag);
                }
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendInt32(0);
                Message.AppendBoolean(true);
                Message.AppendBoolean(false);
                Message.AppendBoolean(true);
                Message.AppendString("");
                Message.AppendBoolean(false);
				Session.SendMessage(Message);
          
                Session.GetClientMessageHandler().method_5(@class.Id, "");

			}
		}
	}
}
