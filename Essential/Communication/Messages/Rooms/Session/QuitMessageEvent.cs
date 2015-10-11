using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
namespace Essential.Communication.Messages.Rooms.Session
{
	internal sealed class QuitMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
            try
            {
                if (Session != null && Session.GetHabbo() != null && Session.GetHabbo().InRoom)
                {
                    Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId).RemoveUserFromRoom(Session, true, false);
                }
            }
            catch { }
		}
	}
}
