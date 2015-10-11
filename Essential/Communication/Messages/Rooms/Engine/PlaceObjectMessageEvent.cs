using System;
using Essential.HabboHotel.GameClients;
using Essential.Util;
using Essential.HabboHotel.Items;
using Essential.Messages;
using Essential.Storage;
using Essential.HabboHotel.Rooms;
using Essential.HabboHotel.SoundMachine;
namespace Essential.Communication.Messages.Rooms.Engine
{
    internal sealed class PlaceObjectMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {
            if (Session != null && Session.GetHabbo() != null)
            {
                Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
                if (@class != null && @class.method_26(Session) && (ServerConfiguration.AllowFurniDrops || !(@class.Owner != Session.GetHabbo().Username)))
                {
                    string text = Event.PopFixedString();
                    string[] array = text.Split(new char[]
				{
					' '
				});
                    if (array[0].Contains("-"))
                    {
                        array[0] = array[0].Replace("-", "");
                    }
                    uint uint_ = 0u;
                    try
                    {
                        uint_ = uint.Parse(array[0]);
                    }
                    catch
                    {
                        return;
                    }
                    UserItem class2 = Session.GetHabbo().GetInventoryComponent().GetItemById(uint_);
                    if (class2 != null)
                    {
                        string text2 = class2.GetBaseItem().InteractionType.ToLower();
                        if (text2 != null && text2 == "dimmer" && @class.method_72("dimmer") >= 1)
                        {
                           // Session.SendNotification("You can only have one moodlight in a room.");
                        }
                        else if (text2 != null && text2 == "jukebox" && @class.method_72("jukebox") >= 1)
                        {
                            /*ServerMessage Message = new ServerMessage(Outgoing.Item1); // Update
                            Message.AppendInt32(23);
                            Session.SendMessage(Message);*/
                        }
                        else
                        {
                            RoomItem RoomItem_;
                            if (array[1].StartsWith(":"))
                            {
                                string text3 = @class.method_98(":" + text.Split(new char[]
							{
								':'
							})[1]);
                                if (text3 == null)
                                {
                                    /*ServerMessage Message = new ServerMessage(Outgoing.Item1); // Update
                                    Message.AppendInt32(11);
                                    Session.SendMessage(Message);
                                    */
                                    return;
                                }
                                RoomItem_ = new RoomItem(class2.uint_0, @class.Id, class2.uint_1, class2.string_0, 0, 0, 0.0, 0, text3, @class, class2.LtdId, class2.LtdCnt, class2.GuildData);
                                if (!@class.method_82(Session, RoomItem_, true, null))

                                {
                                    /*ServerMessage Message = new ServerMessage(Outgoing.Item1); // Update
                                    Message.AppendInt32(11);
                                    Session.SendMessage(Message);
                                    */
                                    goto IL_32C;
                                }
                                Session.GetHabbo().GetInventoryComponent().method_12(uint_, 1u, false);
                                using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
                                {
                                    class3.ExecuteQuery(string.Concat(new object[]
								{
									"UPDATE items SET room_id = '",
									@class.Id,
									"' WHERE Id = '",
									class2.uint_0,
									"' LIMIT 1"
								}));
                                    goto IL_32C;
                                }
                            }
                            int int_ = int.Parse(array[1]);
                            int int_2 = int.Parse(array[2]);
                            int int_3 = int.Parse(array[3]);
                            RoomItem_ = new RoomItem(class2.uint_0, @class.Id, class2.uint_1, class2.string_0, 0, 0, 0.0, 0, "", @class, class2.LtdId, class2.LtdCnt, class2.GuildData);
                            if (@class.method_79(Session, RoomItem_, int_, int_2, int_3, true, false, false))
                            {
                                Session.GetHabbo().GetInventoryComponent().method_12(uint_, 1u, false);
                                using (DatabaseClient class3 = Essential.GetDatabase().GetClient())
                                {
                                    class3.ExecuteQuery(string.Concat(new object[]
								{
									"UPDATE items SET room_id = '",
									@class.Id,
									"' WHERE Id = '",
									class2.uint_0,
									"' LIMIT 1"
								}));
                                }
                            }
                        IL_32C:
                            if (Session.GetHabbo().CurrentQuestId > 0 && Essential.GetGame().GetQuestManager().GetQuestAction(Session.GetHabbo().CurrentQuestId) == "PLACEITEM")
                            {
                                Essential.GetGame().GetQuestManager().ProgressUserQuest(Session.GetHabbo().CurrentQuestId, Session);
                            }
                        }
                    }
                   
                }
                else
                {

                    ServerMessage Message = new ServerMessage(Outgoing.GenericError); // New
                    Message.AppendInt32(-32000);
                    Session.SendMessage(Message);
                }
            }
        }
    }
}