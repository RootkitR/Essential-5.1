using System;
using Essential.Core;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
namespace Essential.Communication.Messages.Rooms.Session
{
    internal sealed class GetHeightmapMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
                RoomData @class = Essential.GetGame().GetRoomManager().method_12(Session.GetHabbo().uint_2);
				if (@class != null)
				{
					if (@class.Model == null)
					{
						Session.SendNotification("Error loading room, please try again soon! (Error Code: MdlData)");
                        Session.SendMessage(new ServerMessage(Outgoing.OutOfRoom)); // P
						Session.GetClientMessageHandler().method_7();
					}
					else
					{
                        Session.SendMessage(@class.Model.method_1(Essential.GetGame().GetRoomManager().GetRoom(@class.Id)));
						Session.SendMessage(@class.Model.method_2(Essential.GetGame().GetRoomManager().GetRoom(@class.Id)));
                        /*if (@class.IsPublicRoom)
                        {
                            if (@class.Model.string_2.Contains("|"))
                            {
                                ServerMessage Message = new ServerMessage(Outgoing.RoomModel); // Updated
                                Message.AppendInt32(@class.Model.string_2.Split('|').Length);
                                foreach (string PublicIt in @class.Model.string_2.Split('|'))
                                {
                                    Message.AppendInt32(0);
                                    Message.AppendString(PublicIt.Split(' ')[0]);
                                    Message.AppendString(PublicIt.Split(' ')[1]);
                                    Message.AppendInt32(int.Parse(PublicIt.Split(' ')[2]));
                                    Message.AppendInt32(int.Parse(PublicIt.Split(' ')[3]));
                                    Message.AppendInt32(int.Parse(PublicIt.Split(' ')[4]));
                                    Message.AppendInt32(int.Parse(PublicIt.Split(' ')[5]));
                                }
                                Session.SendMessage(Message);
                                ServerMessage Message6 = new ServerMessage(Outgoing.RoomEntryInfo); // P
                                Message6.AppendBoolean(false);
                                Message6.AppendString(@class.ModelName);
                                Message6.AppendUInt(0);
                                Message6.AppendBoolean(false);
                                Session.SendMessage(Message6);
                            }
                        }   */               
                    }
                }
        }
    }
}
