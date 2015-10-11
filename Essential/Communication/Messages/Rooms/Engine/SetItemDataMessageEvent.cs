using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Engine
{
	internal sealed class SetItemDataMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
			if (@class != null)
			{
				RoomItem class2 = @class.method_28(Event.PopWiredUInt());
				if (class2 != null && !(class2.GetBaseItem().InteractionType.ToLower() != "postit"))
				{
                    string Color = Event.PopFixedString();
                    string Text = Event.PopFixedString();
                    if (@class.method_26(Session) || Text.StartsWith(class2.ExtraData))
					{
                        switch (Color)
                        {
                            case "FFFF33":
                            case "FF9CFF":
                            case "9CCEFF":
                            case "9CFF9C":
                                break;
                            default:
                                return; // invalid color
                        }
                        class2.ExtraData = Color + " " + Text;
						class2.UpdateState(true, true);
						
					}
				}
			}
		}
	}
}
