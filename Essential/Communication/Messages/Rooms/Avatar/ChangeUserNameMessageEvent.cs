using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.Storage;
namespace Essential.Communication.Messages.Rooms.Avatar
{
	internal sealed class ChangeUserNameMessageEvent : Interface
	{
		[CompilerGenerated]
		private static Func<Room, int> func_0;
		public void Handle(GameClient Session, ClientMessage Event)
		{
			string text = Essential.DoFilter(Event.PopFixedString(), false, true);
			if (text.Length < 3)
			{
                ServerMessage Message = new ServerMessage(Outgoing.ChangeUsernameError); // Updated
                Message.AppendInt32(2);
                Message.AppendString(text);
                Message.AppendInt32(0);
				Session.SendMessage(Message);
			}
			else
			{
				if (text.Length > 15)
				{
                    ServerMessage Message = new ServerMessage(Outgoing.ChangeUsernameError); // Updated
                    Message.AppendInt32(3);
                    Message.AppendString(text);
                    Message.AppendInt32(0);
					Session.SendMessage(Message);
				}
				else
				{
					if (text.Contains(" ") || !Session.GetClientMessageHandler().method_8(text) || text != ChatCommandHandler.ApplyFilter(text))
					{
                        ServerMessage Message = new ServerMessage(Outgoing.ChangeUsernameError); // Updated
                        Message.AppendInt32(5);
                        Message.AppendString(text);
                        Message.AppendInt32(0);
						Session.SendMessage(Message);
					}
					else
					{
                        if (Event.Id == 3511)
						{
                            ServerMessage Message = new ServerMessage(Outgoing.ChangeUsernameError); // Updated
                            Message.AppendInt32(0);
							Message.AppendString(text);
                            Message.AppendInt32(0);
							Session.SendMessage(Message);
						}
						else
						{
                            if (Event.Id == 1457)
							{

                                ServerMessage Message3 = new ServerMessage(Outgoing.ChangeUsername1); // Updated
								Message3.AppendUInt(Session.GetHabbo().Id);
                                Message3.AppendUInt(Session.GetHabbo().CurrentRoomId);
								Message3.AppendString(text);
								Session.SendMessage(Message3);
								if (Session.GetHabbo().CurrentRoomId > 0u)
								{
									Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
									RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
                                    ServerMessage Message4 = new ServerMessage(Outgoing.SetRoomUser); // P
									Message4.AppendInt32(1);
									class2.method_14(Message4);
									@class.SendMessage(Message4, null);
								}
								Dictionary<Room, int> dictionary = Essential.GetGame().GetRoomManager().method_22();
								IEnumerable<Room> arg_204_0 = dictionary.Keys;
								if (ChangeUserNameMessageEvent.func_0 == null)
								{
									ChangeUserNameMessageEvent.func_0 = new Func<Room, int>(ChangeUserNameMessageEvent.smethod_0);
								}
								IOrderedEnumerable<Room> orderedEnumerable = arg_204_0.OrderByDescending(ChangeUserNameMessageEvent.func_0);
								foreach (Room current in orderedEnumerable)
								{
									if (current.Owner == Session.GetHabbo().Username)
									{
										current.Owner = text;
										Essential.GetGame().GetRoomManager().method_16(Essential.GetGame().GetRoomManager().GetRoom(current.Id));
									}
								}
								using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
								{
                                    class3.AddParamWithValue("newuname", text);
                                    class3.AddParamWithValue("olduname", Session.GetHabbo().Username);
									class3.ExecuteQuery(string.Concat(new string[]
									{
										"UPDATE rooms SET owner = @newuname WHERE owner = @olduname"
									}));
                                    class3.AddParamWithValue("newuname", text);
                                    
									class3.ExecuteQuery(string.Concat(new object[]
									{
										"UPDATE users SET username = @newuname WHERE Id = '",
										Session.GetHabbo().Id,
										"' LIMIT 1"
									}));
									Essential.GetGame().GetClientManager().StoreCommand(Session, "flagme", "OldName: " + Session.GetHabbo().Username + " NewName: " + text);
									Session.GetHabbo().Username = text;
									Session.GetHabbo().method_1(class3);
                                    foreach (RoomData current2 in Session.GetHabbo().OwnedRooms)
									{
										current2.Owner = text;
									}
								}
                                Session.GetHabbo().ChangeNamaAchievementsCompleted();
							}
						}
					}
				}
			}
		}
		[CompilerGenerated]
		private static int smethod_0(Room class14_0)
		{
			return class14_0.UserCount;
		}
	}
}
