using System;
using Essential.HabboHotel.GameClients;
using Essential.Messages;
using Essential.HabboHotel.Items;
using Essential.HabboHotel.Rooms;
namespace Essential.Communication.Messages.Wired
{
    internal sealed class UpdateTriggerMessageEvent : Interface
    {
        public void Handle(GameClient Session, ClientMessage Event)
        {

            Room @class = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().CurrentRoomId);
            if (@class != null)
            {
            
            RoomItem class2 = @class.method_28(Event.PopWiredUInt());
            if (class2 != null)
            {
                string text = class2.GetBaseItem().InteractionType.ToLower();
                if (text != null)
                {
                    switch(text)
                    {
                        case "wf_trg_atscore":
                            {
                                Event.PopWiredInt32();
                                class2.string_3 = Event.PopWiredInt32().ToString();
                                goto updatethings;
                            }
                        case "wf_cnd_time_more_than":
                        case "wf_cnd_time_less_than":
                            {
                                string text2 = Event.ToString().Substring(Event.Length - (Event.RemainingLength - 2));
                                string[] array = text2.Split(new char[]
							    {
                                    '@'
                                });
                                class2.string_3 = array[0];
                                class2.string_2 = Convert.ToString(Convert.ToString((double)Event.PopWiredInt32() * 0.5));
                                goto updatethings;
                            }
                        case "wf_trg_attime":
                            {
                                int junk = Event.PopWiredInt32();
                                int junk2 = Event.PopWiredInt32();
                                string message = Event.PopFixedString();
                                class2.string_3 = message;
                                class2.string_2 = Convert.ToString(Convert.ToString((double)junk2 * 0.5));
                                goto updatethings;
                            }
                        case "wf_trg_timer":
                            {
                                Event.PopWiredInt32();
                                string text2 = Event.ToString().Substring(Event.Length - (Event.RemainingLength - 2));
                                string[] array = text2.Split(new char[]
							    {
								    '@'
							    });
                                class2.string_3 = array[0];
                                class2.string_2 = Convert.ToString(Convert.ToString((double)Event.PopWiredInt32() * 0.5));
                                goto updatethings;
                            }
                        case "wf_cnd_actor_in_group":
                        case "wf_trg_enterroom":
                        case "wf_cnd_not_in_group":
                            {
                                Event.PopWiredInt32();
                                string text3 = Event.PopFixedString();
                                class2.string_2 = text3;
                                goto updatethings;
                            }
                        case "wf_trg_onsay":
                            {
                                Event.PopWiredInt32();
                                bool value = (Event.PopWiredInt32() == 1);
                                string text3 = Event.PopFixedString();
                                text3 = Essential.DoFilter(text3, false, true);
                                if (text3.Length > 100)
                                {
                                    text3 = text3.Substring(0, 100);
                                }
                                class2.string_2 = text3;
                                class2.string_3 = Convert.ToString(value);
                                goto updatethings;
                            }

                    }
                }
                updatethings:
                class2.UpdateState(true, false);
                Session.SendMessage(new ServerMessage(Outgoing.SaveWired)); // NEW
            }
        }
        }
    }
}
