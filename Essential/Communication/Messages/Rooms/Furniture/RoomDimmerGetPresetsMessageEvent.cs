using System;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Items;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Rooms.Furniture
{
	internal sealed class RoomDimmerGetPresetsMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			try
			{
				Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null && @class.CheckRights(Session, true) && @class.MoodlightData != null)
				{
                    ServerMessage Message = new ServerMessage(Outgoing.DimmerData); // Updated
					Message.AppendInt32(@class.MoodlightData.Presets.Count);
					Message.AppendInt32(@class.MoodlightData.CurrentPreset);
					int num = 0;
					foreach (MoodlightPreset current in @class.MoodlightData.Presets)
					{
						num++;
						Message.AppendInt32(num);
						Message.AppendInt32(int.Parse(Essential.BooleanToString(current.BackgroundOnly)) + 1);
						Message.AppendStringWithBreak(current.ColorCode);
						Message.AppendInt32(current.ColorIntensity);
					}
					Session.SendMessage(Message);
				}
			}
			catch
			{
			}
		}
	}
}
