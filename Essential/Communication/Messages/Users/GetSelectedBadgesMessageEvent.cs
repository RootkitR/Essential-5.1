using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Users.Badges;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Users
{
	internal sealed class GetSelectedBadgesMessageEvent : Interface
	{
		public void Handle(GameClient session, ClientMessage message)
		{
			if (session != null && session.GetHabbo() != null)
			{
				Room room = Essential.GetGame().GetRoomManager().GetRoom(session.GetHabbo().CurrentRoomId);

				if (room != null)
				{
					RoomUser targetUser = room.GetRoomUserByHabbo(message.PopWiredUInt());

					if (targetUser != null && !targetUser.IsBot && targetUser.GetClient() != null)
					{
                        ServerMessage Message = new ServerMessage(Outgoing.UpdateBadges); // Updated

						Message.AppendUInt(targetUser.GetClient().GetHabbo().Id);
						Message.AppendInt32(targetUser.GetClient().GetHabbo().GetBadgeComponent().VisibleBadges);

						using (TimedLock.Lock(targetUser.GetClient().GetHabbo().GetBadgeComponent().GetBadges()))
						{
							foreach (Badge current in targetUser.GetClient().GetHabbo().GetBadgeComponent().GetBadges())
							{
								if (current.Slot > 0)
								{
									Message.AppendInt32(current.Slot);
									Message.AppendStringWithBreak(current.Code);
								}
							}
						}

						session.SendMessage(Message);
					}
				}
			}
		}
	}
}
