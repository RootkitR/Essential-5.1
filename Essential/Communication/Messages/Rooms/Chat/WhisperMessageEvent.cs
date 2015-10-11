using System;
using Essential.HabboHotel.Misc;
using Essential.HabboHotel.GameClients;
using Essential.Util;
using Essential.Messages;
using Essential.HabboHotel.Rooms;
using Essential.Storage;
namespace Essential.Communication.Messages.Rooms.Chat
{
	internal sealed class WhisperMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
           
			Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null && Session != null && Session.GetHabbo().PassedSafetyQuiz)
			{
                Session.GetHabbo().CheckForUnmute();
				if (Session.GetHabbo().IsMuted)
				{
					Session.SendNotification(EssentialEnvironment.GetExternalText("error_muted"));
				}
				else
				{
                    if (Session.GetHabbo().HasFuse("ignore_roommute") || !@class.bool_4)
                    {
                        string text = Essential.FilterString(Event.PopFixedString());
                        if (!String.IsNullOrEmpty(text) || !String.IsNullOrWhiteSpace(text))
                        {
                            string text2 = text.Split(new char[]
						{
							' '
						})[0];
                            string text3 = text.Substring(text2.Length + 1);
                            text3 = ChatCommandHandler.ApplyFilter(text3);
                            RoomUser class2 = @class.GetRoomUserByHabbo(Session.GetHabbo().Id);
                            RoomUser class3 = @class.method_56(text2);
                            if (Session.GetHabbo().method_4() > 0)
                            {
                                TimeSpan timeSpan = DateTime.Now - Session.GetHabbo().dateTime_0;
                                if (timeSpan.Seconds > 4)
                                {
                                    Session.GetHabbo().int_23 = 0;
                                }
                                if (timeSpan.Seconds < 4 && Session.GetHabbo().int_23 > 5 && !class2.IsBot)
                                {
                                    ServerMessage Message = new ServerMessage(Outgoing.FloodFilter);
                                    Message.AppendInt32(Session.GetHabbo().method_4());
                                    Session.SendMessage(Message);
                                    Session.GetHabbo().IsMuted = true;
                                    Session.GetHabbo().int_4 = Session.GetHabbo().method_4();
                                    return;
                                }
                                Session.GetHabbo().dateTime_0 = DateTime.Now;
                                Session.GetHabbo().int_23++;
                            }
                            ServerMessage Message2 = new ServerMessage(Outgoing.Whisp); // OLD 25 UPDATED
                            Message2.AppendInt32(class2.VirtualId);
                            Message2.AppendStringWithBreak(text3);
                            Message2.AppendInt32(0);
                            Message2.AppendInt32(Event.PopWiredInt32());
                            Message2.AppendInt32(0);
                            Message2.AppendInt32(-1);
                            if (class2 != null && !class2.IsBot && !Essential.GetAntiAd().ContainsIllegalWord(text3))
                            {
                                class2.GetClient().SendMessage(Message2);
                            }
                            class2.Unidle();
                            if (class3 != null && !class3.IsBot && (class3.GetClient().GetHabbo().list_2.Count <= 0 || !class3.GetClient().GetHabbo().list_2.Contains(Session.GetHabbo().Id)))
                            {
                                if(!Essential.GetAntiAd().ContainsIllegalWord(text3))
                                class3.GetClient().SendMessage(Message2);
                                if (ServerConfiguration.EnableChatlog)
                                {
                                    using (DatabaseClient class4 = Essential.GetDatabase().GetClient())
                                    {
                                        class4.AddParamWithValue("message", "<Whisper to " + class3.GetClient().GetHabbo().Username + ">: " + text3);
                                        class4.ExecuteQuery(string.Concat(new object[]
									{
										"INSERT INTO chatlogs (user_id,room_id,hour,minute,timestamp,message,user_name,full_date) VALUES ('",
										Session.GetHabbo().Id,
										"','",
										@class.Id,
										"','",
										DateTime.Now.Hour,
										"','",
										DateTime.Now.Minute,
										"',UNIX_TIMESTAMP(),@message,'",
										Session.GetHabbo().Username,
										"','",
										DateTime.Now.ToLongDateString(),
										"')"
									}));
                                    }
                                }
                                if (Essential.GetAntiAd().ContainsIllegalWord(text3))
                                {
                                    ServerMessage Message3 = new ServerMessage(Outgoing.InstantChat);
                                    Message3.AppendUInt(0u);
                                    Message3.AppendString("[AWS] " + Session.GetHabbo().Username + ": " + text3);
                                    Message3.AppendString(Essential.GetUnixTimestamp() + string.Empty);
                                    Essential.GetGame().GetClientManager().SendToStaffs(Session, Message3);
                                    Session.SendNotification(Essential.GetGame().GetRoleManager().GetConfiguration().getData("antiad.alert"));
                                }
                            }
                        }
                    }
				}
			}
		}
	}
}
