using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
namespace Essential.Communication.Messages.Users
{
	internal sealed class UnignoreUserMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room class14_ = Session.GetHabbo().CurrentRoom;
			if (class14_ != null)
			{
				string string_ = Event.PopFixedString();
				RoomUser @class = class14_.method_56(string_);
				if (@class != null)
				{
					uint uint_ = @class.GetClient().GetHabbo().Id;
					if (Session.GetHabbo().list_2.Contains(uint_))
					{
						Session.GetHabbo().list_2.Remove(uint_);
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
							class2.ExecuteQuery(string.Concat(new object[]
							{
								"DELETE FROM user_ignores WHERE user_id = ",
								Session.GetHabbo().Id,
								" AND ignore_id = ",
								uint_,
								" LIMIT 1;"
							}));
						}
                        ServerMessage Message = new ServerMessage(Outgoing.UpdateIgnoreStatus); // Updated
						Message.AppendInt32(3);
                        Message.AppendString(string_);
						Session.SendMessage(Message);
					}
				}
			}
		}
	}
}
