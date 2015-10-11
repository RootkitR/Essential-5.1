using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
namespace Essential.Communication.Messages.Users
{
	internal sealed class IgnoreUserMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			Room Room = Session.GetHabbo().CurrentRoom;
            if (Room != null)
			{
				string string_ = Event.PopFixedString();
                RoomUser @class = Room.method_56(string_);
				if (@class != null)
				{
					uint uint_ = @class.GetClient().GetHabbo().Id;
					if (!Session.GetHabbo().list_2.Contains(uint_))
					{
						Session.GetHabbo().list_2.Add(uint_);
						using (DatabaseClient class2 = Essential.GetDatabase().GetClient())
						{
							class2.ExecuteQuery(string.Concat(new object[]
							{
								"INSERT INTO user_ignores(user_id, ignore_id) VALUES (",
								Session.GetHabbo().Id,
								", ",
								uint_,
								");"
							}));
						}
                        ServerMessage Message = new ServerMessage(Outgoing.UpdateIgnoreStatus); // Updated
                        Message.AppendInt32(1);
                        Message.AppendString(string_);
						Session.SendMessage(Message);
					}
				}
			}
		}
	}
}
