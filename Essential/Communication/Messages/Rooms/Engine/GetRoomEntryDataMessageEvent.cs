using System;
using System.Collections;
using System.Collections.Generic;
using Essential.HabboHotel.GameClients;
using Essential.HabboHotel.Rooms;
using Essential.Messages;
using Essential.HabboHotel.Items;
namespace Essential.Communication.Messages.Rooms.Engine
{
	internal sealed class GetRoomEntryDataMessageEvent : Interface
	{
		public void Handle(GameClient Session, ClientMessage Event)
		{
			if (Session.GetHabbo().uint_2 > 0u && Session.GetHabbo().bool_5)
			{
                RoomData @class = Essential.GetGame().GetRoomManager().method_12(Session.GetHabbo().uint_2);
				if (@class != null)
				{
			
						Room class2 = Essential.GetGame().GetRoomManager().GetRoom(Session.GetHabbo().uint_2);
						if (class2 != null)
						{
							Session.GetClientMessageHandler().method_7();
                           
							if (class2.Type == "private")
							{
								Hashtable hashtable_ = class2.Hashtable_0;
								Hashtable hashtable_2 = class2.Hashtable_1;
                                ServerMessage Message2 = new ServerMessage(Outgoing.SerializeFloorItems); // P
                                if (hashtable_.Count > 0)
                                {
                                    Message2.AppendInt32(1);
                                    Message2.AppendInt32(class2.OwnerId);
                                    Message2.AppendString(class2.Owner);
                                }
                                else
                                {
                                    Message2.AppendInt32(0);
                                }
								Message2.AppendInt32(hashtable_.Count);
								foreach (RoomItem class3 in hashtable_.Values)
								{
                                    class3.Serialize(Message2);
								}
                               /* using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\filut\testit.txt", true))
                                {
                                    file.WriteLine(Message2.ToString());
                                }
                                */
								Session.SendMessage(Message2);
                                ServerMessage Message3 = new ServerMessage(Outgoing.SerializeWallItems); // P
                                if (hashtable_2.Count > 0)
                                {
                                    Message3.AppendInt32(1);
                                    Message3.AppendInt32(class2.OwnerId);
                                    Message3.AppendString(class2.Owner);
                                }
                                else
                                {
                                    Message3.AppendInt32(0);
                                }
								Message3.AppendInt32(hashtable_2.Count);
								foreach (RoomItem class3 in hashtable_2.Values)
								{
									class3.Serialize(Message3);
								}
								Session.SendMessage(Message3);
							}
                            else
                            {

                            }
							class2.method_46(Session, Session.GetHabbo().bool_8);
							List<RoomUser> list = new List<RoomUser>();
							for (int i = 0; i < class2.RoomUsers.Length; i++)
							{
								RoomUser class4 = class2.RoomUsers[i];
								if (class4 != null && (!class4.bool_11 && class4.bool_12))
								{
									list.Add(class4);
								}
							}
                            ServerMessage Message4 = new ServerMessage(Outgoing.SetRoomUser); // P
							Message4.AppendInt32(list.Count);
							foreach (RoomUser class4 in list)
							{
								class4.method_14(Message4);
							}
							Session.SendMessage(Message4);
							if (class2.Type == "public")
							{
                             

                           
                            /*        ServerMessage Message6 = new ServerMessage(2877); // P
                                    Message6.AppendBoolean(false);
                                    Message6.AppendString(class2.ModelName);
                                    Message6.AppendUInt(0);
                                    Message6.AppendBoolean(false);
                                    Session.SendMessage(Message6);
                                */
                               
							}
							else
							{
								if (class2.Type == "private")
								{
                                    ServerMessage Message5 = new ServerMessage(Outgoing.ConfigureWallandFloor); // P
                                    Message5.AppendBoolean(class2.Hidewall);
                                    Message5.AppendInt32(class2.Wallthick);
                                    Message5.AppendInt32(class2.Floorthick);
                                    Session.SendMessage(Message5);

                                    ServerMessage Message6 = new ServerMessage(Outgoing.RoomEntryInfo); // P
									Message6.AppendBoolean(true);
									Message6.AppendUInt(class2.Id);
                                    Message6.AppendBoolean(class2.CheckRights(Session, true));
									Session.SendMessage(Message6);
                                    ServerMessage RoomDataa = new ServerMessage(Outgoing.RoomData);
                                    RoomDataa.AppendBoolean(true);
                                    RoomDataa.AppendInt32(class2.Id);
                                    RoomDataa.AppendString(class2.Name);
                                    RoomDataa.AppendBoolean(true);
                                    RoomDataa.AppendInt32(class2.OwnerId);
                                    RoomDataa.AppendString(class2.Owner);
                                    RoomDataa.AppendInt32(class2.State);
                                    RoomDataa.AppendInt32(class2.UsersNow);
                                    RoomDataa.AppendInt32(class2.UsersMax);
                                    RoomDataa.AppendString(class2.Description);
                                    RoomDataa.AppendInt32((class2.Category == 0x34) ? 2 : 0);
                                    RoomDataa.AppendInt32(0);
                                    RoomDataa.AppendInt32(class2.Score);
                                    RoomDataa.AppendInt32(0);
                                    RoomDataa.AppendInt32(class2.Category);
                                    if (Groups.GetRoomGroup(class2.Id) == null)
                                    {
                                        RoomDataa.AppendInt32(0);
                                        RoomDataa.AppendInt32(0);
                                    }
                                    else
                                    {
                                        GroupsManager guild = Groups.GetRoomGroup(class2.Id);
                                        RoomDataa.AppendInt32(guild.Id);
                                        RoomDataa.AppendString(guild.Name);
                                        RoomDataa.AppendString(guild.Badge);
                                    }
                                    RoomDataa.AppendString("");
                                    RoomDataa.AppendInt32(class2.Tags.Count);
                                    foreach (string str in class2.Tags)
                                    {
                                        RoomDataa.AppendString(str);
                                    }
                                    RoomDataa.AppendInt32(0);
                                    RoomDataa.AppendInt32(0);
                                    RoomDataa.AppendInt32(0);
                                    RoomDataa.AppendBoolean(true);
                                    RoomDataa.AppendBoolean(true);
                                    RoomDataa.AppendInt32(0);
                                    RoomDataa.AppendInt32(0);
                                    RoomDataa.AppendBoolean(false);
                                    RoomDataa.AppendBoolean(false);
                                    RoomDataa.AppendBoolean(false);
                                    RoomDataa.AppendInt32(0);
                                    RoomDataa.AppendInt32(0);
                                    RoomDataa.AppendInt32(0);
                                    RoomDataa.AppendBoolean(false);
                                    RoomDataa.AppendBoolean(true);
                                    RoomDataa.AppendBoolean(true);//forse
                                    Session.SendMessage(RoomDataa);
                                    if((class2.UsersWithRights.Count > 0) && class2.CheckRights(Session, true))
                                    {
                                        ServerMessage PowerList = new ServerMessage(Outgoing.GetPowerList);
                                        PowerList.AppendInt32(class2.RoomData.Id);
                                        PowerList.AppendInt32(class2.UsersWithRights.Count);
                                        foreach (uint num in class2.UsersWithRights)
                                        {
                                            PowerList.AppendInt32(num);
                                            PowerList.AppendString(Essential.GetGame().GetClientManager().GetNameById(num));
                                        }
                                        Session.SendMessage(PowerList);
                                        foreach (uint num in class2.UsersWithRights)
                                        {
                                            ServerMessage GivePower = new ServerMessage(Outgoing.GivePowers);
                                            GivePower.AppendInt32(class2.RoomData.Id);
                                            GivePower.AppendInt32(num);
                                            GivePower.AppendString(Essential.GetGame().GetClientManager().GetNameById(num));
                                            Session.SendMessage(GivePower);
                                        }
                                    }
								}
							}
							ServerMessage Message8 = class2.method_67(true);
							if (Message8 != null)
							{
								Session.SendMessage(Message8);
							}
							for (int i = 0; i < class2.RoomUsers.Length; i++)
							{
								RoomUser class4 = class2.RoomUsers[i];
								if (class4 != null && !class4.bool_11)
								{
									if (class4.IsDancing)
									{
                                        ServerMessage Message9 = new ServerMessage(Outgoing.Dance); // 480 Dance Updated
										Message9.AppendInt32(class4.VirtualId);
										Message9.AppendInt32(class4.DanceId);
										Session.SendMessage(Message9);
									}
									if (class4.bool_8)
									{
                                        ServerMessage Message10 = new ServerMessage(Outgoing.IdleStatus); // 486 IdleStatus Updated
										Message10.AppendInt32(class4.VirtualId);
										Message10.AppendBoolean(true);
										Session.SendMessage(Message10);
									}
									if (class4.CarryItemID > 0 && class4.int_6 > 0)
									{
                                        ServerMessage Message11 = new ServerMessage(Outgoing.ApplyCarryItem); // CarryItem Updated
										Message11.AppendInt32(class4.VirtualId);
										Message11.AppendInt32(class4.CarryItemID);
										Session.SendMessage(Message11);
									}
									if (!class4.IsBot)
									{
										try
										{
											if (class4.GetClient().GetHabbo() != null && class4.GetClient().GetHabbo().GetEffectsInventoryComponent() != null && class4.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0 >= 1)
											{
                                                ServerMessage Message12 = new ServerMessage(Outgoing.ApplyEffects); // 485 ApplyEffects Updated
												Message12.AppendInt32(class4.VirtualId);
												Message12.AppendInt32(class4.GetClient().GetHabbo().GetEffectsInventoryComponent().int_0);
                                                Message12.AppendInt32(0);
												Session.SendMessage(Message12);
											}
											goto IL_5C5;
										}
										catch
										{
											goto IL_5C5;
										}
									}
									if (!class4.IsPet && class4.RoomBot.EffectId != 0)
									{
                                        ServerMessage Message12 = new ServerMessage(Outgoing.ApplyEffects);
										Message12.AppendInt32(class4.VirtualId);
										Message12.AppendInt32(class4.RoomBot.EffectId);
                                        Message12.AppendInt32(0);
										Session.SendMessage(Message12);
									}
								}
								IL_5C5:;
							}
							if (class2 != null && Session != null && Session.GetHabbo().CurrentRoom != null)
							{
                               // Session.GetHabbo().GetEffectsInventoryComponent().method_2(0, true);
								class2.method_8(Session.GetHabbo().CurrentRoom.GetRoomUserByHabbo(Session.GetHabbo().Id));
							}
							if (class2.Achievement > 0u)
							{
								Essential.GetGame().GetAchievementManager().addAchievement(Session, class2.Achievement, 1);
							}
                            /*
                            ServerMessage InitTrade = new ServerMessage(Outgoing.TradeAllowed);
                            InitTrade.AppendInt32(1);
                            Session.SendMessage(InitTrade);
                            */
							if (Session.GetHabbo().IsMuted && Session.GetHabbo().int_4 > 0)
							{
								/*ServerMessage Message13 = new ServerMessage(27u);
								Message13.AppendInt32(Session.GetHabbo().int_4);
								Session.SendMessage(Message13);
                                */
							}
						}
					}
				
			}
		}
	}
}
